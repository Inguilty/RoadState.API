using System;

namespace RoadState.BusinessLayer.TransportModels
{
    public class CommentDto: BaseDto
    {
        public string AuthorName { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
