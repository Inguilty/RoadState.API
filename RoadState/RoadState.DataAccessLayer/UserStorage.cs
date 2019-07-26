using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RoadState.DataAccessLayer
{
    public interface IUserCreator
    {
        void CreateUser(User user);
    }

    public interface IUserFinder
    {
        IEnumerable<User> GetUsers(Expression<Func<User, bool>> predicate);
    }

    public interface IUserUpdator
    {
        void UpdateUser(User user);
    }

    public class UserStorage : IUserCreator, IUserFinder, IUserUpdator
    {
        private RoadStateContext _context;
        public UserStorage(RoadStateContext context)
        {
            this._context = context;
        }
        public void CreateUser(User user)
        {
            this._context.Users.AddAsync(user);
            this._context.SaveChangesAsync();
        }

        public IEnumerable<User> GetUsers(Expression<Func<User, bool>> predicate)
        {
            return this._context.Users.Where(predicate);
        }

        public void UpdateUser(User user)
        {
            this._context.Users.Update(user);
            this._context.SaveChangesAsync();
        }
    }
}
