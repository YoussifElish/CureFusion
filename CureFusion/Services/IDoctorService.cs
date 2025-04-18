using CureFusion.Abstactions;
using CureFusion.Contracts.Doctor;

namespace CureFusion.Services;

public interface IDoctorService
{
    Task<Result> RegisterAsDoctor (DoctorRegisterRequest request,string userId, CancellationToken cancellationToken = default!);
}
