using System;
using System.Collections.Generic;
using System.Text;

namespace RoadState.Data
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Location HomeLocation { get; set; }
    }
}
