using System;
using ATMSystem.Interfaces;
using ATMSystem.Misc;

namespace ATMSystem.Objects
{
    public class Track : ITrack
    {
        public string Tag { get; set; }
        public ICoordinate CurrentPosition { get; set; }
        public ICoordinate LastKnownPosition { get; set; }
        public int CurrentAltitude { get; set; }
        public int CurrentHorizontalVelocity { get; set; }
        public int CurrentCompassCourse { get; set; }
        public DateTime LastSeen { get; set; }
        public IDirectionCalc DirectionCalc { get; set; }

        private int CalculateHorizontalVelocity(ICoordinate coordinate, DateTime timestamp)
        {
            try
            {
                var horizontalVelocity = Math.Abs(LastKnownPosition.x - CurrentPosition.x / (int)LastSeen.Subtract(timestamp).TotalSeconds);
                return horizontalVelocity;
            }
            catch (System.DivideByZeroException)
            {
                return CurrentHorizontalVelocity;
            }
        }

        public void Update(ICoordinate coordinate, int altitude, DateTime timestamp)
        {
            LastKnownPosition = CurrentPosition;
            CurrentPosition = coordinate;
            CurrentAltitude = altitude;
            CurrentHorizontalVelocity = CalculateHorizontalVelocity(coordinate, timestamp);
            LastSeen = timestamp;
            CurrentCompassCourse = DirectionCalc.CalculateDirection(LastKnownPosition, CurrentPosition);
        }

        public Track()
        {
            Tag = "UNSET";
            CurrentCompassCourse = 0;
            CurrentHorizontalVelocity = 0;
            CurrentPosition = new Coordinate {x = 0, y = 0};
            LastKnownPosition = new Coordinate {x = 0, y = 0};
            DirectionCalc = new DirectionCalc();
            LastSeen = DateTime.Now;
        }

        public Track(string tag)
        {
            Tag = tag;
            CurrentCompassCourse = 0;
            CurrentHorizontalVelocity = 0;
            CurrentPosition = new Coordinate {x = 0, y = 0};
            LastKnownPosition = new Coordinate {x = 0, y = 0};
            DirectionCalc = new DirectionCalc();
            LastSeen = DateTime.Now;
        }

        public Track(string tag, ICoordinate currentPos)
        {
            Tag = tag;
            CurrentCompassCourse = 0;
            CurrentHorizontalVelocity = 0;
            CurrentPosition = currentPos;
            LastKnownPosition = new Coordinate {x = 0, y = 0};
            DirectionCalc = new DirectionCalc();
            LastSeen = DateTime.Now;
        }
    }
}