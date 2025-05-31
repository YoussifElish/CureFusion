using CureFusion.Application.Contracts.Doctor;
using CureFusion.Application.Contracts.Files;
using CureFusion.Domain.Abstactions;
using CureFusion.Domain.Entities;

namespace CureFusion.Application.Services;

public interface IDoctorService
{
    Task<Result> RegisterAsDoctor(DoctorRegisterRequest request, RegisterDoctorImageRequest imageRequest, CancellationToken cancellationToken = default);
    Task<Result> DoctorAvaliability(DoctorAvailabilityRequest request, CancellationToken cancellationToken = default!);
    Task<Result> DeleteDoctorAppointment(int Id, CancellationToken cancellationToken = default);
    Task<Result> DeleteDoctorAvaliability(int Id, CancellationToken cancellationToken = default);
    Task<Result<List<Appointment>>> GetDoctorAppointments(CancellationToken cancellationToken = default);
    Task<Result<List<DoctorAvailability>>> GetDoctorAvailabilities(CancellationToken cancellationToken = default);

}
