using System;

namespace ATMSystem.Interfaces
{
    public interface ITrack
    {
        string Tag { get; set; }
        ICoordinate CurrentPosition { get; set; }
        ICoordinate LastKnownPosition { get; set; }
        int CurrentAltitude { get; set; }
        int CurrentHorizontalVelocity { get; set; }
        int CurrentCompassCourse { get; set; }
        void Update(ICoordinate coord, int altitude, DateTime timestamp);
        DateTime LastSeen { get; set; }

    }
}