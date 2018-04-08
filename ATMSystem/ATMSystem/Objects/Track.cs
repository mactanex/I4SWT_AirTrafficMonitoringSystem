using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;
using ATMSystem.Misc;

namespace ATMSystem.Objects
{
    public class Track : ITrack
    {
        public string Tag { get; set; }
        public ICoordinate CurrentPosition { get ; set; }
        public ICoordinate LastKnownPosition { get; set; }
        public int CurrentAltitude { get; set; }
        public int CurrentHorizontalVelocity { get ; set; }
        public int CurrentCompassCourse { get; set; }
        public DateTime LastSeen { get; set; }
        public IDirectionCalc DirectionCalc { get; set; }

        public void UpdatePosition(ICoordinate coordinate, DateTime timestamp)
        {
            LastKnownPosition = CurrentPosition;
            CurrentPosition = coordinate;
            CurrentHorizontalVelocity = LastKnownPosition.x - CurrentPosition.x / (int)(LastSeen.Subtract(timestamp).TotalSeconds);
            LastSeen = timestamp;
            CurrentCompassCourse = DirectionCalc.CalculateDirection(LastKnownPosition, CurrentPosition);
        }

        public Track()
        {
            Tag = "UNSET";
            CurrentCompassCourse = 0;
            CurrentHorizontalVelocity = 0;
            CurrentPosition = new Coordinate();
            LastKnownPosition = new Coordinate();
            DirectionCalc = new DirectionCalc();

        }

        public Track(string tag)
        {
            Tag = tag;
            CurrentCompassCourse = 0;
            CurrentHorizontalVelocity = 0;
            CurrentPosition = new Coordinate();
            LastKnownPosition = new Coordinate();
            DirectionCalc = new DirectionCalc();
        }

        public Track(string tag, ICoordinate currentPos)
        {
            Tag = tag;
            CurrentCompassCourse = 0;
            CurrentHorizontalVelocity = 0;
            CurrentPosition = currentPos;
            LastKnownPosition = new Coordinate();
            DirectionCalc = new DirectionCalc();
        }
    }
}
