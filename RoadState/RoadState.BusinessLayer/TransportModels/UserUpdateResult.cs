namespace RoadState.BusinessLayer.TransportModels
{
    public class UserUpdateResult
    {
        public bool ErrorOccured { get; set; } = false;
        public string ErrorMessage { get; set; }
    }
}
