namespace CureFusion.Services;

public interface IGeoCodingService
{
    Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string zone);
}
