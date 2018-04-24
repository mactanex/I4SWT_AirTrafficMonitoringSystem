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
        private const int MaxAllowedVerticalDistance = 300;
        private const int MaxAllowedHorizontalDistance = 5000;

        private readonly List<ISeperation> _seperations;
        public IReadOnlyCollection<ISeperation> Seperations => _seperations;
        public event EventHandler<SeperationEventArgs> OnSeperationEvent;
        public void TrackDataHandler(object obj, TrackControllerEventArgs args)
        {
            var track = args.TrackTag;
            CalculateSeperation(track);
        }

        private readonly ITrackController _trackController;

        public SeperationMonitor(ITrackController trackController)
        {
            _trackController = trackController as ITrackController;
            if (_trackController != null) _trackController.OnTrackUpdated += TrackDataHandler;
            _seperations = new List<ISeperation>();
        }

        public void CalculateSeperation(string trackTag)
        {
            var currentSeperations = FindSeperations(trackTag);

            if (currentSeperations.Count > 0)
            {
                foreach (var seperation in currentSeperations)
                {
                    if (!IsConflicting(seperation.TrackOne, seperation.TrackTwo))
                    {
                        seperation.ConflictingSeperation = false;
                        seperation.TimeOfOccurence = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                        OnSeperationEvent?.Invoke(null, new SeperationEventArgs(seperation));
                        _seperations.Remove(seperation);
                    }
                }
            }

            _trackController.Tracks.TryGetValue(trackTag, out var track);

            if (track != null)
            {
                foreach (ITrack targetTrack in _trackController.Tracks.Values)
                {
                    if (targetTrack.Tag != track.Tag && IsConflicting(track, targetTrack) && !SeperationExists(track, targetTrack))
                    {
                        var sep = new Seperation
                        {
                            ConflictingSeperation = true,
                            TimeOfOccurence = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                            TrackOne = track,
                            TrackTwo = targetTrack
                        };
                        _seperations.Add(sep);
                        OnSeperationEvent?.Invoke(null, new SeperationEventArgs(sep));
                    }
                }
            }
        }

        List<ISeperation> FindSeperations(string tag)
        {
            List<ISeperation> current = _seperations.Where(s => s.TrackOne.Tag == tag || s.TrackTwo.Tag == tag).ToList();
            return current;
        }

        bool IsConflicting(ITrack trackOne, ITrack trackTwo)
        {
            if (CalculateVerticalDistance(trackOne.CurrentAltitude, trackTwo.CurrentAltitude) < MaxAllowedVerticalDistance)
            {
                if (CalculateHorisontalDistance(trackOne.CurrentPosition, trackTwo.CurrentPosition) < MaxAllowedHorizontalDistance)
                {
                    return true;
                }
            }
            return false;
        }

        double CalculateHorisontalDistance(ICoordinate firstCoordinate, ICoordinate secondCoordinate)
        {
            var deltaX = Math.Abs(firstCoordinate.x - secondCoordinate.x);
            var deltaY = Math.Abs(firstCoordinate.y - secondCoordinate.y);
            return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
        }

        double CalculateVerticalDistance(int firstAltitude, int secondAlitutde)
        {
            return Math.Abs(firstAltitude - secondAlitutde);
        }

        bool SeperationExists(ITrack trackOne, ITrack trackTwo)
        {
            var result = false;
            foreach (var seperation in _seperations)
            {
                if ((seperation.TrackOne.Tag == trackOne.Tag && seperation.TrackTwo.Tag == trackTwo.Tag) 
                    || (seperation.TrackOne.Tag == trackTwo.Tag && seperation.TrackTwo.Tag == trackOne.Tag))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

    }
}
