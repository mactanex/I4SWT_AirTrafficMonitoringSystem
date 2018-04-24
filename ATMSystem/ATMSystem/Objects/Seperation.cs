using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Objects
{
    public class Seperation : ISeperation
    {
        public string TimeOfOccurence { get; set; }
        public ITrack TrackOne { get; set; }
        public ITrack TrackTwo { get; set; }
        public bool ConflictingSeperation { get; set; }
    }
}
