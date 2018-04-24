using System;
using System.Collections.Generic;

namespace ATMSystem.Interfaces
{
    public class SeparationEventArgs : EventArgs
    {
        public ISeparation Separation { get; set; }
        public SeparationEventArgs(ISeparation separation)
        {
            Separation = separation;
        }
    }
    public interface ISeparationMonitor
    {
        event EventHandler<SeparationEventArgs> OnSeparationEvent;
        void TrackDataHandler(object obj, TrackControllerEventArgs args);
        void CalculateSeparation(string trackTag);
        IReadOnlyCollection<ISeparation> Separations { get; }

    }
}