using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.BusinessLayer.Shared.TransportModels
{
    public class UserTransportModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
        public string Token { get; set; }
    }
}
