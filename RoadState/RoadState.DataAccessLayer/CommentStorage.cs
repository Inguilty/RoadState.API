using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoadState.DataAccessLayer
{
    public interface ICommentCreator
    {
        Task CreateCommentAsync(Comment comment);
    }

    public interface ICommentLiker
    {
        Task LikeCommentAsync(Comment comment, User user, bool hasLiked);
    }

    public class CommentStorage : ICommentCreator, ICommentLiker
    {
        private RoadStateContext _context;
        public CommentStorage(RoadStateContext context)
        {
            this._context = context;
        }
        public async Task CreateCommentAsync(Comment comment)
        {
            await this._context.Comments.AddAsync(comment);
            await this._context.SaveChangesAsync();
        }

        public async Task LikeCommentAsync(Comment comment, User user, bool hasLiked)
        {
            await this._context.UserLikes.AddAsync(new UserMark
            {
                Comment = comment,
                CommentId = comment.Id,
                User = user,
                UserId = user.Id,
                HasLiked = hasLiked,
            });
            await this._context.SaveChangesAsync();
        }
    }
}
