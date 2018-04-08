using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;
using ATMSystem.Objects;
using TransponderReceiver;

namespace ATMSystem.Handlers
{
    class TrackController : ITrackController
    {
        public event EventHandler OnTransponderDataReady;

        private readonly Dictionary<string, ITrack> _tracks = new Dictionary<string, ITrack>();
        public IReadOnlyDictionary<string, ITrack> Tracks => _tracks;

        // Grid boundaries
        private readonly ICoordinate _swCornorCoordinate;
        private readonly ICoordinate _neCornerCoordinate;
        private readonly int _lowerAltitudeBoundary;
        private readonly int _upperAltitudeBoundary;

        public TrackController(ITransponderReceiver transponderReceiver)
        {
            _swCornorCoordinate = new Coordinate() {x = 10000, y = 10000};
            _neCornerCoordinate = new Coordinate() {x = 90000, y = 90000};
            _lowerAltitudeBoundary = 500;
            _upperAltitudeBoundary = 20000;

            transponderReceiver.TransponderDataReady += TransponderDataHandler;
        }

        public void TransponderDataHandler(object obj, EventArgs args)
        {
            var data = args as RawTransponderDataEventArgs;

            ICoordinate cord = new Coordinate()
            {
                //x = data[]
            };

            // Track is already in the list
            if (_tracks.ContainsKey(data.TransponderData[0]))
            {

            }

        }

        public ITrack ConvertRawDataToTrack(string rawData)
        {
            var values = new List<String>();
            int position = 0;
            int start = 0;
            do
            {
                position = rawData.IndexOf(';', start);
                if (position >= 0)
                {
                    values.Add(rawData.Substring(start, position - start + 1).Trim());
                    start = position + 1;
                }
            } while (position > 0);


        }

        private bool CheckBoundary(ICoordinate coordinate, int altitude)
        {
            if (coordinate.x < _swCornorCoordinate.x || coordinate.x > _neCornerCoordinate.x)
                return false;

            if (coordinate.y < _swCornorCoordinate.y || coordinate.y > _neCornerCoordinate.y)
                return false;

            if (altitude < _lowerAltitudeBoundary || altitude > _upperAltitudeBoundary)
                return false;

            return true;
        }
    }
}
