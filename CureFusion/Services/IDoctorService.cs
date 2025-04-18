using CureFusion.Abstactions;
using CureFusion.Contracts.Doctor;

namespace CureFusion.Services;

public interface IDoctorService
{
    Task<Result> RegisterAsDoctor (DoctorRegisterRequest request,string userId, CancellationToken cancellationToken = default!);
    Task<Result> DoctorAvaliability(DoctorAvailabilityRequest request , string userId, CancellationToken cancellationToken = default!);
}
