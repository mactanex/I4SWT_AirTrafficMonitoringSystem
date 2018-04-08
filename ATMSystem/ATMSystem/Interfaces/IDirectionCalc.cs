namespace ATMSystem.Interfaces
{
    public interface IDirectionCalc
    {
        //maybe create a Coordinate class containing the two coordinates
        int CalculateDirection(ICoordinate oldCoordinate, ICoordinate newCoordinate);
    }
}