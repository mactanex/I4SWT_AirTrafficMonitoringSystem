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
    public class SeparationMonitor : ISeparationMonitor
    {
        private const int MaxAllowedVerticalDistance = 300;
        private const int MaxAllowedHorizontalDistance = 5000;

        private readonly List<ISeparation> _seperations;
        public IReadOnlyCollection<ISeparation> Separations => _seperations;
        public event EventHandler<SeparationEventArgs> OnSeparationEvent;
        public void TrackDataHandler(object obj, TrackControllerEventArgs args)
        {
            var track = args.TrackTag;
            CalculateSeparation(track);
        }

        private readonly ITrackController _trackController;

        public SeparationMonitor(ITrackController trackController)
        {
            _trackController = trackController as ITrackController;
            if (_trackController != null) _trackController.OnTrackUpdated += TrackDataHandler;
            _seperations = new List<ISeparation>();
        }

        public void CalculateSeparation(string trackTag)
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
                        OnSeparationEvent?.Invoke(null, new SeparationEventArgs(seperation));
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
                        var sep = new Separation
                        {
                            ConflictingSeperation = true,
                            TimeOfOccurence = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                            TrackOne = track,
                            TrackTwo = targetTrack
                        };
                        _seperations.Add(sep);
                        OnSeparationEvent?.Invoke(null, new SeparationEventArgs(sep));
                    }
                }
            }
        }

        List<ISeparation> FindSeperations(string tag)
        {
            List<ISeparation> current = _seperations.Where(s => s.TrackOne.Tag == tag || s.TrackTwo.Tag == tag).ToList();
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
