using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.Data
{
    public class UserLikes : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public bool HasLiked { get; set; }
    }
}
