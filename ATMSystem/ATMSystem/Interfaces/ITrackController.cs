using System;
using System.Collections.Generic;

namespace ATMSystem.Interfaces
{
    public interface ITrackController
    {
        event EventHandler OnTrackUpdated;
        void TransponderDataHandler(object obj, EventArgs args);
        IReadOnlyDictionary<string, ITrack> Tracks { get; }
    }
}