using System;
using System.Globalization;

namespace RoadState.BusinessLayer.Helpers
{
    public class WrongCredentialsException : Exception
    {
        public WrongCredentialsException() : base() { }

        public WrongCredentialsException(string message) : base(message) { }

        public WrongCredentialsException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
