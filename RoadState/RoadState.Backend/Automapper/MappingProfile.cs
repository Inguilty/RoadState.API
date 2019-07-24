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
                .ForMember("AuthorName", opt => opt.MapFrom(b => _context.Users.Find(b.AuthorId).UserName))
                .ForMember("Location", opt => opt.MapFrom(b => new Location() { Longitude = b.Longitude, Latitude = b.Latitude }))
                .ForMember(b=>b.Comments, opt=> opt.MapFrom(b=>b.Comments.Select(Mapper.Map<Comment, CommentDTO>).ToList()));
            CreateMap<Comment, CommentDTO>()
                .ForMember("AuthorName", opt => opt.MapFrom(c => _context.Users.Find(c.AuthorId).UserName))
                .ForMember("Likes", opt => opt.MapFrom(c => _context.UserLikes.Where(x => x.CommentId == c.Id && x.HasLiked).Count()))
                .ForMember("Dislikes", opt => opt.MapFrom(c => _context.UserLikes.Where(x => x.CommentId == c.Id && !x.HasLiked).Count()));
        }
    }
}
