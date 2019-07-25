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
        private readonly RoadStateContext _context;
        public MappingProfile(RoadStateContext context)
        {
            _context = context;
            CreateMap<BugReport, BugReportDTO>()
                .ForMember("AuthorName", opt => opt.MapFrom(b => b.Author.UserName))
                .ForMember("Location", opt => opt.MapFrom(b => new Location() { Longitude = b.Longitude, Latitude = b.Latitude }));

            CreateMap<Comment, CommentDTO>()
                .ForMember("AuthorName", opt => opt.MapFrom(c => c.Author.UserName))
                .ForMember("Likes", opt => opt.MapFrom(c => c.UserLikes.Where(x => x.HasLiked).Count()))
                .ForMember("Dislikes", opt => opt.MapFrom(c => c.UserLikes.Where(x => !x.HasLiked).Count()));


        }
    }
}
