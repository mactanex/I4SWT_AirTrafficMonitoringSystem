namespace ATMSystem.Interfaces
{
    public interface ISeparation
    {
        string TimeOfOccurence { get; set; }
        ITrack TrackOne { get; set; }
        ITrack TrackTwo { get; set; }
        bool ConflictingSeperation { get; set; }
    }
}