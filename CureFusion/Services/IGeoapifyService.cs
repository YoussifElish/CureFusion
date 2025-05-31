using CureFusion.Contracts.Articles;

namespace CureFusion.Services;

public interface IGeoapifyService
{
    Task<PageinatedList<Hospital>> GetNearbyHospitalsAsync(double latitude, double longitude, int radius = 5000, int pageNumber = 1, int pageSize = 10);





}
