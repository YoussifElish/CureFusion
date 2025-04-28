using CureFusion.Abstactions;
using CureFusion.Contracts.Doctor;
using CureFusion.Contracts.Files;

namespace CureFusion.Services;

public interface IDoctorService
{
    Task<Result> RegisterAsDoctor(DoctorRegisterRequest request, RegisterDoctorImageRequest imageRequest, CancellationToken cancellationToken = default);
    Task<Result> DoctorAvaliability(DoctorAvailabilityRequest request , CancellationToken cancellationToken = default!);
    Task<Result> DeleteDoctorAvaliability(int Id,  CancellationToken cancellationToken = default!);
}
