using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.BusinessLayer
{
    public class CommentDTO: BaseDTO
    {
        public string AuthorName { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}
