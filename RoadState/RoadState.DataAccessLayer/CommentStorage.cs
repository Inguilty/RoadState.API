using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.DataAccessLayer
{
    public interface ICommentCreator
    {
        void CreateComment(Comment comment);
    }

    public interface ICommentLiker
    {
        void LikeComment(Comment comment, User user, bool hasLiked);
    }

    public class CommentStorage : ICommentCreator, ICommentLiker
    {
        private RoadStateContext _context;
        public CommentStorage(RoadStateContext context)
        {
            this._context = context;
        }
        public void CreateComment(Comment comment)
        {
            this._context.Comments.AddAsync(comment);
            this._context.SaveChangesAsync();
        }

        public void LikeComment(Comment comment, User user, bool hasLiked)
        {
            this._context.UserLikes.AddAsync(new UserMark
            {
                Comment = comment,
                CommentId = comment.Id,
                User = user,
                UserId = user.Id,
                HasLiked = hasLiked,
            });
            this._context.SaveChangesAsync();
        }
    }
}
