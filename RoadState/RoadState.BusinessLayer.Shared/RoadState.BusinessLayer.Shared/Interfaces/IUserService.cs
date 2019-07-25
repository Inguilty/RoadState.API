using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoadState.BusinessLayer.Shared.TransportModels;

namespace RoadState.BusinessLayer.Shared.Interfaces
{
    public interface IUserService
    {
        Task<UserTransportModel> Authenticate(string username, string password);
        Task<UserTransportModel> GetById(string id);
        Task<UserTransportModel> Create(UserTransportModel user, string password);
        void Update(UserTransportModel user, string password = null);
    }
}
