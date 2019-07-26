using AutoMapper;
using RoadState.BusinessLayer;
using RoadState.Data;
using RoadState.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadState.Backend.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<BugReport, BugReportDto>()
                .ForMember(b=>b.AuthorName, opt => opt.MapFrom(b => b.Author.UserName))
                .ForMember(b=>b.Location, opt => opt.MapFrom(b => new Location() { Longitude = b.Longitude, Latitude = b.Latitude }));
            CreateMap<Comment, CommentDto>()
                .ForMember(c=>c.AuthorName, opt => opt.MapFrom(c => c.Author.UserName))
                .ForMember(c=>c.Likes, opt => opt.MapFrom(c => c.UserLikes.FindAll(x=>x.HasLiked).Count))
                .ForMember(c=>c.Dislikes, opt => opt.MapFrom(c => c.UserLikes.FindAll(x=>!x.HasLiked).Count));
        }
    }
}
