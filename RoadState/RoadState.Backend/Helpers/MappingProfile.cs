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
            CreateMap<RoadState.Data.User, UserTransportModel>();
            CreateMap<UserTransportModel, RoadState.Data.User>();
            CreateMap<RoadState.Backend.Models.User, UserTransportModel>();
            CreateMap<UserTransportModel, RoadState.Backend.Models.User>();
        }
    }
}
