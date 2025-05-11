namespace CureFusion.Services;

public interface IGeoCoding
{
    Task<Location> GetCoordinatesAsync(string address);
}

