using RoadState.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.DataAccessLayer.Interfaces
{
    public interface ICommentLiker
    {
        void LikeComment(Comment comment, User user, bool hasLiked);
    }
}
