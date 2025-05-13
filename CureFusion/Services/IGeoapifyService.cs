namespace CureFusion.Services;

public interface IGeoapifyService
{
    Task<List<Hospital>> GetNearbyHospitalsAsync(double latitude, double longitude, int radius = 10000);
}
