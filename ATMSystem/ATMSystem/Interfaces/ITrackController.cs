using System;
using System.Collections.Generic;

namespace ATMSystem.Interfaces
{
    public class TrackControllerEventArgs : EventArgs
    {
        public string TrackTag { get; set; }

        public TrackControllerEventArgs(string trackTag)
        {
            TrackTag = trackTag;
        }
    }

    public interface ITrackController
    {
        event EventHandler<TrackControllerEventArgs> OnTrackUpdated;
        void TransponderDataHandler(object obj, EventArgs args);
        IReadOnlyDictionary<string, ITrack> Tracks { get; }
    }
}