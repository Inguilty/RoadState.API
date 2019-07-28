using System;
using System.Globalization;

namespace RoadState.BusinessLayer.Helpers
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base() { }

        public UserNotFoundException(string message) : base(message) { }

        public UserNotFoundException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
