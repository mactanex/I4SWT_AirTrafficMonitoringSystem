namespace ATMSystem.Interfaces
{
    public interface IOutput
    {
        void WriteToOutput(ITrack track);

        void Clear();
    }
}