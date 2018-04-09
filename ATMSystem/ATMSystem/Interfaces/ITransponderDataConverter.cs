using System;

namespace ATMSystem.Interfaces
{
    public interface ITransponderDataConverter
    {
        ITrack GetTrack(string rawdata);
    }
}