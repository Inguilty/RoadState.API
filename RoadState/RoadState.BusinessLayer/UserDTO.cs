using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.BusinessLayer
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
