using AutoMapper;
using RoadState.BusinessLayer.Shared.TransportModels;
using RoadState.Data;
using RoadState.Backend.Models;
using RoadState.BusinessLayer;

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
            CreateMap<User, UserDto>();
            CreateMap<BugReport, BugReportDto>()
                .ForMember(b => b.AuthorName, opt => opt.MapFrom(b => b.Author.UserName))
                .ForMember(b => b.Location, opt => opt.MapFrom(b => new Location() { Longitude = b.Longitude, Latitude = b.Latitude }));
            CreateMap<Comment, CommentDto>()
                .ForMember(c => c.AuthorName, opt => opt.MapFrom(c => c.Author.UserName))
                .ForMember(c => c.Likes, opt => opt.MapFrom(c => c.UserLikes.FindAll(x => x.HasLiked).Count))
                .ForMember(c => c.Dislikes, opt => opt.MapFrom(c => c.UserLikes.FindAll(x => !x.HasLiked).Count));
        }
    }
}
