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

        public ITrack GetTrack(string rawdata)
        {
            try
            {
                var values = SerializeData(rawdata);

                ITrack track = new Track()
                {
                    Tag = values[0],
                    CurrentPosition = new Coordinate()
                    {x = int.Parse(values[1]), y = int.Parse(values[2])},
                    CurrentAltitude = int.Parse(values[3])
                };

                return track;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private DateTime GetTimeStamp(string timestamp)
        {
            try
            {
                string format = "yyyyMMddHHmmssfff";
                DateTime time = DateTime.ParseExact(timestamp, format, CultureInfo.InvariantCulture);
                return time;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private List<string> SerializeData(string rawData)
        {
            var result = rawData.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim())
                .ToList();

            return result.Count != 5 ? null : result;
        }
    }
}