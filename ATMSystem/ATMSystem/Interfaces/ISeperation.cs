namespace ATMSystem.Interfaces
{
    public interface ISeperation
    {
        string TimeOfOccurence { get; set; }
        ITrack Track1 { get; set; }
        ITrack Track2 { get; set; }


    }
}