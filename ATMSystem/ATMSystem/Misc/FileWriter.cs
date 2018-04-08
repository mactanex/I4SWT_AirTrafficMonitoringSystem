using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;

//  inspired by https://stackoverflow.com/questions/20185015/how-to-write-log-file-in-c
namespace ATMSystem.Misc
{
    public class FileWriter : IFileWriter
    {
        private string _path = string.Empty;

        public void Write(string str)
        {
            _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            try
            {
                using (StreamWriter w = File.AppendText(_path + "\\" + "log.txt"))
                {
                    w.Write(str);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
