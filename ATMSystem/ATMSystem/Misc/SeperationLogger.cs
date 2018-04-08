using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

namespace ATMSystem.Misc
{
    //  inspired by https://stackoverflow.com/questions/20185015/how-to-write-log-file-in-c
    public class SeperationLogger : ISeperationLogger
    {
        private string _path = string.Empty;
        
        /// <summary>
        /// Logs seperation to file
        /// </summary>
        /// <param name="seperation"></param>
        public void LogSeperation(ISeperation seperation)
        {
            _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            try
            {
                using (StreamWriter w = File.AppendText(_path+"\\"+"log.txt"))
                {
                    w.Write("\r\nLog Entry: ");
                    w.WriteLine(seperation.TimeOfOccurence);
                    w.WriteLine("Tags involved: ");
                    w.WriteLine(seperation.TagsOfInvolvedTracks);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
