using AutoMapper;
using RoadState.BusinessLayer.Shared.TransportModels;
using RoadState.Data;
using RoadState.Backend.Models;

namespace RoadState.Backend.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserTransportModel>();
            CreateMap<UserTransportModel, User>();
            CreateMap<UserModel, UserTransportModel>();
            CreateMap<UserTransportModel, UserModel>();
        }
    }
}
