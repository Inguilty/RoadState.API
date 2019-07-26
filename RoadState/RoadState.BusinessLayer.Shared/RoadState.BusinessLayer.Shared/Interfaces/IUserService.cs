using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RoadState.BusinessLayer.Shared.TransportModels;

namespace RoadState.BusinessLayer.Shared.Interfaces
{
    public interface IUserService
    {
        Task<UserTransportModel> Authenticate(string username, string password, string appSettings);
        Task<UserTransportModel> GetById(string id);
        Task<UserTransportModel> Create(UserTransportModel user, string password);
        Task<bool> Update(UserTransportModel user, string newPassword);
    }
}
