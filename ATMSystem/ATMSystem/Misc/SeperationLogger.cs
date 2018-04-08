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
        public SeperationLogger()
        {
            FileWriter = new FileWriter();
        }

        /// <summary>
        /// Logs seperation to file
        /// </summary>
        /// <param name="seperation"></param>
        public void LogSeperation(ISeperation seperation)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\r\nLog Entry: ");
            sb.AppendLine();
            sb.Append("tags involved: ");
            sb.AppendLine(seperation.Track1.Tag + " : " + seperation.Track2.Tag);

            FileWriter.Write(sb.ToString());
        }

        
    }
}
