using CureFusion.Abstactions;

namespace CureFusion.Services;

public class DoctorService : IDoctorService
{
    public Task<Result> RegisterAsDoctor(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
