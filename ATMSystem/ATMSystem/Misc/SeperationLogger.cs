﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Misc
{
    
    public class SeperationLogger : ISeperationLogger
    {
        /// <summary>
        /// FileWriter
        /// </summary>
        private IFileWriter FileWriter { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SeperationLogger(IFileWriter fileWriter)
        {
            FileWriter = fileWriter;
        }

        /// <summary>
        /// Logs seperation to file
        /// </summary>
        /// <param name="seperation"></param>
        public void LogSeperation(ISeperation seperation)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\r\nLog Entry: " + seperation.TimeOfOccurence);
            sb.AppendLine();
            sb.Append("tags involved: " + seperation.Track1.Tag + " : " + seperation.Track2.Tag);
            sb.AppendLine();

            FileWriter.Write(sb.ToString());
        }

        
    }
}
