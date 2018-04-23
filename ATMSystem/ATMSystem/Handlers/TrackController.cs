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
    public class TrackController : ITrackController
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
        public IOutput Output { get; set; }

        public TrackController(ITransponderReceiver transponderReceiver, ITransponderDataConverter dataConverter, IOutput output)
        {
            _swCornorCoordinate = new Coordinate() {x = 10000, y = 10000};
            _neCornerCoordinate = new Coordinate() {x = 90000, y = 90000};
            _lowerAltitudeBoundary = 500;
            _upperAltitudeBoundary = 20000;

            DataConverter = dataConverter;
            Output = output;

            transponderReceiver.TransponderDataReady += TransponderDataHandler;
        }

        public void TransponderDataHandler(object obj, EventArgs args)
        {
            var data = args as RawTransponderDataEventArgs;

            // Update according to received data
            

            foreach (var rawData in data.TransponderData)
            {
                var track = DataConverter.GetTrack(rawData);

                if (track != null)
                {
                    if (!_tracks.ContainsKey(track.Tag))
                    {
                        if(CheckBoundary(track.CurrentPosition, track.CurrentAltitude))
                        {
                            _tracks.Add(track.Tag, track);
                            OnTrackUpdated?.Invoke(track, EventArgs.Empty);
                        }
                    }
                    else
                    {
                        if (CheckBoundary(track.CurrentPosition, track.CurrentAltitude))
                        {
                            _tracks[track.Tag].Update(track.CurrentPosition, track.CurrentAltitude, track.LastSeen);
                            OnTrackUpdated?.Invoke(track, EventArgs.Empty);
                        }
                        else
                        {
                            _tracks.Remove(track.Tag);
                        }
                    }
                }
            }

            // Print all current tracks
            Output.Clear();
            foreach (var track in _tracks.Values)
            {
                Output.WriteToOutput(track);
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
