using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ATMSystem.Interfaces;
using ATMSystem.Objects;
using TransponderReceiver;

namespace ATMSystem.Misc
{
    public class TransponderDataConverter : ITransponderDataConverter
    {
        public DateTime GetTimeStamp(string rawdata)
        {
            try
            {
                var values = SerializeData(rawdata);
                string format = "yyyyMMddHHmmssfff";
                DateTime time = DateTime.ParseExact(values[4], format, CultureInfo.InvariantCulture);
                return time;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        public ITrack GetTrack(string rawdata)
        {
            try
            {
                var values = SerializeData(rawdata);
                ITrack track = new Track(values[0])
                {
                    CurrentPosition = new Coordinate()
                    {x = int.Parse(values[1]), y = int.Parse(values[2])},
                    CurrentAltitude = int.Parse(values[3])
                };

                return track;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
        }

        private List<string> SerializeData(string rawData)
        {
            char[] delimeters = {';'};
            var result = rawData.Split(delimeters, StringSplitOptions.RemoveEmptyEntries).ToList();
            if(result.Count != 5)
                throw new ArgumentException();
            return result;
        }
    }
}