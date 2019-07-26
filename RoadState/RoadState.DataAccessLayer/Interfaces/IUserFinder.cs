using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace RoadState.DataAccessLayer.Interfaces
{
    public interface IUserFinder
    {
        IEnumerable<User> GetUsers(Expression<Func<User, bool>> predicate);
    }
}
