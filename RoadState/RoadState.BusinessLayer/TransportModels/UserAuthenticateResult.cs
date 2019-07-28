namespace RoadState.BusinessLayer.TransportModels
{
    public class UserAuthenticateResult
    {
        public bool ErrorOccured { get; set; } = false;
        public string ErrorMessage { get; set; }
        public UserDto User { get; set; }
    }
}
