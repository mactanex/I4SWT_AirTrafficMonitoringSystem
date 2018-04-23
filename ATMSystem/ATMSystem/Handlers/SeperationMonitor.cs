using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMSystem.Interfaces;
using ATMSystem.Objects;

namespace ATMSystem.Handlers
{
    public class SeperationMonitor : ISeperationMonitor
    {
        public event EventHandler OnSeperationEvent;

        private readonly ITrackController _trackController;

        public SeperationMonitor(ITrackController trackController)
        {
            _trackController = trackController as ITrackController;
            if (_trackController != null) _trackController.OnTrackUpdated += TrackDataHandler;
        }

        public void TrackDataHandler(object obj, EventArgs args)
        {
            var track = obj as ITrack;
            ISeperation possibleSeperation = CalculateSeperation(track);
            if (possibleSeperation != null)
            {
                OnSeperationEvent?.Invoke(possibleSeperation, null);
            }
        }

        public ISeperation CalculateSeperation(ITrack track)
        {
            foreach (ITrack element  in _trackController.Tracks.Values)
            {
                if (element.Tag != track.Tag)
                {
                    if (Math.Abs(track.CurrentAltitude - element.CurrentAltitude) < 300)
                    {
                        var deltaX = Math.Abs(track.CurrentPosition.x - element.CurrentPosition.x);
                        var deltaY = Math.Abs(track.CurrentPosition.y - element.CurrentPosition.y);

                        if (Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2)) < 5000)
                        {
                            var seperation = new Seperation();
                            seperation.TimeOfOccurence = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                            seperation.Track1 = track;
                            seperation.Track2 = element;
                            return seperation;
                        }
                    }
                }
            }

            return null;
        }
    }
}
