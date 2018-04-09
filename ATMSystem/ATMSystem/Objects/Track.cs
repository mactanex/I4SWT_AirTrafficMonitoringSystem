using System;
using ATMSystem.Interfaces;
using ATMSystem.Misc;

namespace ATMSystem.Objects
{
    public class Track : ITrack
    {
        private string _tag;

        public string Tag
        {
            get => _tag;
            set
            {
                if(value == null || value.Length != 6)
                    throw new ArgumentException();

                _tag = value;
            }
        }

        public ICoordinate CurrentPosition { get; set; }
        public ICoordinate LastKnownPosition { get; set; }
        public int CurrentAltitude { get; set; }
        public double CurrentHorizontalVelocity { get; set; }
        public int CurrentCompassCourse { get; set; }
        public DateTime LastSeen { get; set; }
        public IDirectionCalc DirectionCalc { get; set; }

        private double CalculateHorizontalVelocity(DateTime timestamp)
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
            CurrentHorizontalVelocity = CalculateHorizontalVelocity(timestamp);
            LastSeen = timestamp;
            CurrentCompassCourse = DirectionCalc.CalculateDirection(LastKnownPosition, CurrentPosition);
        }

        public Track(IDirectionCalc calc = null)
        {
            DirectionCalc = calc ?? new DirectionCalc();
        }

        public Track(string tag, ICoordinate currentPos, int altitude, DateTime timestamp, IDirectionCalc calc = null)
        {
            Tag = tag;
            CurrentAltitude = altitude;
            CurrentPosition = currentPos;
            LastKnownPosition = currentPos;
            DirectionCalc = calc ?? new DirectionCalc();
            LastSeen = timestamp;
        }
    }
}