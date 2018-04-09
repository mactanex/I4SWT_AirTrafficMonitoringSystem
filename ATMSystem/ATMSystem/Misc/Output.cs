using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Misc
{
    public class Output : IOutput
    {
        public void WriteToOutput(ITrack track)
        {
            Console.WriteLine("Tag: " + track.Tag);
            Console.WriteLine("Current Position: (" + track.CurrentPosition.x + "," + track.CurrentPosition.y + ")" );
            Console.WriteLine("Current Altitude: " + track.CurrentAltitude);
            Console.WriteLine("Current Horizontal Velocity: " + track.CurrentHorizontalVelocity);
            Console.WriteLine("Current Compass Course: " + track.CurrentCompassCourse);
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
