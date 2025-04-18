using CureFusion.Abstactions;

namespace CureFusion.Services;

public interface IDoctorService
{
    Task<Result> RegisterAsDoctor (CancellationToken cancellationToken = default!);
}
