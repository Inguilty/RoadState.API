using RoadState.Data;
using RoadState.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.DataAccessLayer
{
    public class CommentStorage : ICommentCreator, ICommentLiker
    {
        private RoadStateContext _context;
        public CommentStorage(RoadStateContext context)
        {
            this._context = context;
        }
        public void CreateComment(Comment comment)
        {
            this._context.Comments.Add(comment);
            this._context.SaveChanges();
        }

        public void LikeComment(Comment comment, User user, bool hasLiked)
        {
            this._context.UserLikes.Add(new UserLike
            {
                Comment = comment,
                CommentId = comment.Id,
                User = user,
                UserId = user.Id,
                HasLiked = hasLiked,
            });
            this._context.SaveChanges();
        }
    }
}
