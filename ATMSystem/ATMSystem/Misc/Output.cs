using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Misc
{
    class Output : IOutput
    {
        public void WriteToOutput(ITrack track)
        {
            Console.WriteLine("Tag: " + track.Tag);
            Console.WriteLine("Currrent Position: (" + track.CurrentPosition.x + "," + track.CurrentPosition.y + ")" );
            Console.WriteLine("Current altitude: " + track.CurrentAltitude);
            Console.WriteLine("Current horizontal velocity: " + track.CurrentHorizontalVelocity);
            Console.WriteLine("Current compass course: " + track.CurrentCompassCourse);
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
