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
        public double CurrentHorizontalVelocity { get; set; }
        public int CurrentCompassCourse { get; set; }
        public DateTime LastSeen { get; set; }
        public IDirectionCalc DirectionCalc { get; set; }

        private double CalculateHorizontalVelocity(ICoordinate coordinate, DateTime timestamp)
        {
            var time = timestamp.Subtract(LastSeen);
            var timeTotal = (int) Math.Round(time.TotalSeconds);
            var deltaX = CurrentPosition.x - LastKnownPosition.x;
            var deltaY = CurrentPosition.y - LastKnownPosition.y;
            var horizontalVelocity = Math.Sqrt(Math.Pow(deltaX,2) + Math.Pow(deltaY,2)) / timeTotal;
            if (double.IsInfinity(horizontalVelocity) || double.IsNaN(horizontalVelocity) )
            {
                return CurrentHorizontalVelocity;
            }

                return horizontalVelocity;
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
            Tag = "AAAAAA";
            CurrentCompassCourse = 0;
            CurrentHorizontalVelocity = 0;
            CurrentPosition = new Coordinate {x = 0, y = 0};
            LastKnownPosition = new Coordinate {x = 0, y = 0};
            DirectionCalc = new DirectionCalc();
            LastSeen = DateTime.Now;
        }

        public Track(string tag)
        {
            Tag = tag.Length == 6 ? tag : "AAAAAA";
            CurrentCompassCourse = 0;
            CurrentHorizontalVelocity = 0;
            CurrentPosition = new Coordinate {x = 0, y = 0};
            LastKnownPosition = new Coordinate {x = 0, y = 0};
            DirectionCalc = new DirectionCalc();
            LastSeen = DateTime.Now;
        }

        public Track(string tag, ICoordinate currentPos)
        {
            Tag = tag.Length == 6 ? tag : "AAAAAA";
            CurrentCompassCourse = 0;
            CurrentHorizontalVelocity = 0;
            CurrentPosition = currentPos;
            LastKnownPosition = new Coordinate {x = 0, y = 0};
            DirectionCalc = new DirectionCalc();
            LastSeen = DateTime.Now;
        }

        public Track(string tag, ICoordinate currentPos, int altitude, DateTime timestamp)
        {
            Tag = tag.Length == 6 ? tag : "AAAAAA";
            CurrentCompassCourse = 0;
            CurrentHorizontalVelocity = 0;
            CurrentAltitude = altitude;
            CurrentPosition = currentPos;
            LastKnownPosition = new Coordinate { x = 0, y = 0 };
            DirectionCalc = new DirectionCalc();
            LastSeen = timestamp;
        }
    }
}