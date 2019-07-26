using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.DataAccessLayer.Interfaces
{
    public interface IUserCreator
    {
        void CreateUser(User user);
    }
}
