using Microsoft.EntityFrameworkCore;
using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RoadState.DataAccessLayer
{
    public interface IUserCreator
    {
        Task CreateUserAsync(User user);
    }

    public interface IUserFinder
    {
        Task<List<User>> GetUsersAsync(Expression<Func<User, bool>> predicate);
    }

    public interface IUserUpdator
    {
        Task UpdateUserAsync(User user);
    }

    public class UserStorage : IUserCreator, IUserFinder, IUserUpdator
    {
        private RoadStateContext _context;
        public UserStorage(RoadStateContext context)
        {
            this._context = context;
        }
        public async Task CreateUserAsync(User user)
        {
            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsersAsync(Expression<Func<User, bool>> predicate)
        {
            return await this._context.Users.Where(predicate).ToListAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            this._context.Users.Update(user);
            await this._context.SaveChangesAsync();
        }
    }
}
