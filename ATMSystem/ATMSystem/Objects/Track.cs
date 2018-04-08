using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

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

        public Track(string tag)
        {
            Tag = tag;
        }
    }
}
