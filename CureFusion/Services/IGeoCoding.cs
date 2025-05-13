namespace CureFusion.Services;

public interface IGeoCoding
{
      Task<(double Latitude, double Longitude)> GetCoordinatesAsync(string zone);
}

