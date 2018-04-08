namespace ATMSystem.Interfaces
{
    public interface ITrack
    {
        string Tag { get; set; }
        string CurrentPosition { get; set; }
        int CurrentAltitude { get; set; }
        int CurrentHorizontalVelocity { get; set; }
        int CurrentCompassCourse { get; set; }
    }
}