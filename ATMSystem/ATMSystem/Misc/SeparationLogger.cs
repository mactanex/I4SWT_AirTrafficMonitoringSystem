using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Handlers;
using ATMSystem.Interfaces;

namespace ATMSystem.Misc
{
    
    public class SeparationLogger : ISeparationLogger
    {
        /// <summary>
        /// FileWriter
        /// </summary>
        private IFileWriter FileWriter { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SeparationLogger(IFileWriter fileWriter, ISeparationMonitor separationMonitor)
        {
            FileWriter = fileWriter;
            separationMonitor.OnSeparationEvent += SeparationHandler;
        }

        public void SeparationHandler(object obj, SeparationEventArgs args)
        {
            var seperation = args.Separation;
            LogSeperation(seperation);
        }

        /// <summary>
            /// Logs separation to file
            /// </summary>
            /// <param name="separation"></param>
            public void LogSeperation(ISeparation separation)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\r\nLog Entry: " + separation.TimeOfOccurence);
                sb.AppendLine();
                sb.Append("Tags involved: " + separation.TrackOne.Tag + " : " + separation.TrackTwo.Tag + "\r\n");
                sb.Append("Are " + (separation.ConflictingSeperation ? "conflicting!" : "no longer conflicting!"));
                sb.AppendLine();

                FileWriter.Write(sb.ToString());
            }
    }
}
