using CureFusion.Abstactions;

namespace CureFusion.Errors;

public static class DoctorErrors
{
    public static readonly Error Pending = new("DoctorErrors.Pending", "Your account is still pending approval.", StatusCodes.Status400BadRequest);
    public static readonly Error RegisteredBefore = new("DoctorErrors.RegisteredBefore", "Your account is already active as a doctor", StatusCodes.Status400BadRequest);
    public static readonly Error Removed = new("DoctorErrors.Removed", "You request to remove your account before you must contact support to activate your account again", StatusCodes.Status400BadRequest);

}
