﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.BusinessLayer
{
    public class BugReportDto: BaseDto
    {
        public string AuthorName { get; set; }
        public int Rating { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
        public string UserRate { get; set; }
        public DateTime PublishDate { get; set; }
        public Location Location { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
