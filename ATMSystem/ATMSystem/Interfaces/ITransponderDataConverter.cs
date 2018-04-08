using System;

namespace ATMSystem.Interfaces
{
    public interface ITransponderDataConverter
    {
        DateTime GetTimeStamp(string rawdata);
        ITrack GetTrack(string rawdata);
    }
}