using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Objects
{
    public class Coordinate : ICoordinate
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}
