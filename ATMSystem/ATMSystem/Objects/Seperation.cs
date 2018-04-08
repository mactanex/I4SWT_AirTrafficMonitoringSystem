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
        public ITrack Track1 { get; set; }
        public ITrack Track2 { get; set; }
    }
}
