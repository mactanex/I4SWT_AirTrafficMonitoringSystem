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

        public int CurrentAltitude
        {
            get => CurrentPosition.y;
            set => CurrentPosition.y = value;
        }

        public int CurrentHorizontalVelocity { get; set; }
        public int CurrentCompassCourse
        {
            get => DirectionCalc.CalculateDirection(LastKnownPosition, CurrentPosition);
            set { throw new NotImplementedException(); }
        }

        public IDirectionCalc DirectionCalc { get; set; }



        public void UpdatePosition(ICoordinate coordinate)
        {
            LastKnownPosition = CurrentPosition;
            CurrentPosition = coordinate;
        }

        public Track(string tag, ICoordinate currentPos)
        {
            Tag = tag;
            CurrentAltitude = currentPos.y;
            CurrentCompassCourse = 0;
            CurrentHorizontalVelocity = 0;
            CurrentPosition = currentPos;
            LastKnownPosition = new Coordinate();
            DirectionCalc = new DirectionCalc();
        }
    }
}
