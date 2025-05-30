﻿using CureFusion.Domain.Abstactions;
using Microsoft.AspNetCore.Http;

namespace CureFusion.Domain.Errors;

public static class DoctorErrors
{
    public static readonly Error Pending = new("DoctorErrors.Pending", "Your account is still pending approval.", StatusCodes.Status400BadRequest);
    public static readonly Error RegisteredBefore = new("DoctorErrors.RegisteredBefore", "Your account is already active as a doctor", StatusCodes.Status400BadRequest);
    public static readonly Error Removed = new("DoctorErrors.Removed", "You request to remove your account before you must contact support to activate your account again", StatusCodes.Status400BadRequest);
    public static readonly Error NotDoctor = new("DoctorErrors.NotDoctor", "You Must be a doctor to create new appoitment", StatusCodes.Status203NonAuthoritative);
    public static readonly Error Duplicated = new("DoctorErrors.Duplicated", "you have active session in this period", StatusCodes.Status203NonAuthoritative);
    public static readonly Error NotFound = new("DoctorErrors.NotFound", "Can't Find Any Session with mentioned Id", StatusCodes.Status400BadRequest);
    public static readonly Error AlreadyDeleted = new("DoctorErrors.AlreadyDeleted", "this Appoitment has been deleted before", StatusCodes.Status400BadRequest);
    public static readonly Error NotYourSession = new("DoctorErrors.AlreadyDeleted", "you must to be appoitment owner to delete it", StatusCodes.Status203NonAuthoritative);
    public static readonly Error SessionEnded = new("DoctorErrors.SessionEnded", "can't delete ended session", StatusCodes.Status400BadRequest);

}
