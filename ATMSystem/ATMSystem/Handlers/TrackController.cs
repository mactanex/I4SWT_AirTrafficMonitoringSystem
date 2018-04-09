using System;
using System.Collections.Generic;
using System.Globalization;
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
        public event EventHandler OnTrackUpdated;

        private readonly Dictionary<string, ITrack> _tracks = new Dictionary<string, ITrack>();
        public IReadOnlyDictionary<string, ITrack> Tracks => _tracks;

        // Grid boundaries
        private readonly ICoordinate _swCornorCoordinate;
        private readonly ICoordinate _neCornerCoordinate;
        private readonly int _lowerAltitudeBoundary;
        private readonly int _upperAltitudeBoundary;

        public ITransponderDataConverter DataConverter { get; set; }

        public TrackController(ITransponderReceiver transponderReceiver, ITransponderDataConverter dataConverter)
        {
            _swCornorCoordinate = new Coordinate() {x = 10000, y = 10000};
            _neCornerCoordinate = new Coordinate() {x = 90000, y = 90000};
            _lowerAltitudeBoundary = 500;
            _upperAltitudeBoundary = 20000;

            DataConverter = dataConverter;

            transponderReceiver.TransponderDataReady += TransponderDataHandler;
        }

        public void TransponderDataHandler(object obj, EventArgs args)
        {
            var data = args as RawTransponderDataEventArgs;

            foreach (var rawData in data.TransponderData)
            {
                var track = DataConverter.GetTrack(rawData);

                if (track != null)
                {
                    if (!_tracks.ContainsKey(track.Tag))
                    {
                        _tracks.Add(track.Tag, track);
                    }
                    else
                    {
                        if (CheckBoundary(track.CurrentPosition, track.CurrentAltitude))
                        {
                            _tracks[track.Tag].Update(track.CurrentPosition, track.CurrentAltitude, track.LastSeen);
                        }
                        else
                        {
                            _tracks.Remove(track.Tag);
                        }
                    }
                }
            }


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
