using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;
using ATMSystem.Misc;

namespace ATMSystem.Objects
{
    class Track : ITrack
    {
        public string Tag { get; set; }
        public ICoordinate CurrentPosition { get ; set; }
        public ICoordinate LastKnownPosition { get; set; }

        public int CurrentAltitude { get; set; }

        public int CurrentHorizontalVelocity { get ; set; }
        public int CurrentCompassCourse
        {
            get => DirectionCalc.CalculateDirection(LastKnownPosition, CurrentPosition);
            set
            {
                CurrentCompassCourse = value;
            }
        }

        public IDirectionCalc DirectionCalc { get; set; }

        public void UpdatePosition(ICoordinate coordinate, DateTime timestamp)
        {
            LastKnownPosition = CurrentPosition;
            CurrentPosition = coordinate;
            Timestamp = timestamp;

        }

        public DateTime Timestamp { get; set; }

        public Track()
        {
            

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
