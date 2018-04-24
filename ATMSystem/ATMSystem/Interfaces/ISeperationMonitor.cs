using System;
using System.Collections.Generic;

namespace ATMSystem.Interfaces
{
    public class SeperationEventArgs : EventArgs
    {
        public ISeperation Seperation { get; set; }
        public SeperationEventArgs(ISeperation seperation)
        {
            Seperation = seperation;
        }
    }
    public interface ISeperationMonitor
    {
        event EventHandler<SeperationEventArgs> OnSeperationEvent;
        void TrackDataHandler(object obj, TrackControllerEventArgs args);
        ISeperation CalculateSeperation(string trackTag);
        IReadOnlyCollection<ISeperation> Seperations { get; }

    }
}