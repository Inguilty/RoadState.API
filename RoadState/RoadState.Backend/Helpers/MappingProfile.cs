using AutoMapper;
using RoadState.Data;
using RoadState.Backend.Models;
using RoadState.BusinessLayer.TransportModels;

namespace RoadState.Backend.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ForMember(x=>x.Password, opt => opt.NullSubstitute("sth"));
            CreateMap<UserDto, User>();
            CreateMap<UserProfile, UserDto>();
            CreateMap<UserDto, UserProfile>();
            CreateMap<BugReport, BugReportDto>()
                .ForMember(b => b.AuthorName, opt => opt.MapFrom(b => b.Author.UserName))
                .ForMember(b => b.Location, opt => opt.MapFrom(b => new Location() { Longitude = b.Longitude, Latitude = b.Latitude }));
            CreateMap<Comment, CommentDto>()
                .ForMember(c => c.AuthorName, opt => opt.MapFrom(c => c.Author.UserName))
                .ForMember(c => c.Likes, opt => opt.MapFrom(c => c.UserLikes.FindAll(x => x.HasLiked).Count))
                .ForMember(c => c.Dislikes, opt => opt.MapFrom(c => c.UserLikes.FindAll(x => !x.HasLiked).Count));
            CreateMap<byte[], Photo>()
                .ConvertUsing(b => new Photo() { Blob = b });
            CreateMap<Photo, byte[]>()
                .ConvertUsing(b => b.Blob);
            CreateMap<CreateBugReportDto, BugReport>()
                .ForMember(b => b.State, opt => opt.MapFrom(b => b.ProblemLevel))
                .ForMember(b => b.AuthorId, opt => opt.MapFrom(b => b.UserId));
        }
    }
}
