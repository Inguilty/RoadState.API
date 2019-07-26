using RoadState.Data;
using RoadState.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RoadState.DataAccessLayer
{
    public class UserStorage : IUserCreator, IUserFinder, IUserUpdator
    {
        private RoadStateContext _context;
        public UserStorage(RoadStateContext context)
        {
            this._context = context;
        }
        public void CreateUser(User user)
        {
            this._context.Users.Add(user);
            this._context.SaveChanges();
        }

        public IEnumerable<User> GetUsers(Expression<Func<User, bool>> predicate)
        {
            return this._context.Users.Where(predicate);
        }

        public void UpdateUser(User user)
        {
            this._context.Users.Update(user);
            this._context.SaveChanges();
        }
    }
}
