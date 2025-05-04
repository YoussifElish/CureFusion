using CureFusion.Abstactions;

namespace CureFusion.Errors;

public static class CommonErrors
{

    public static readonly Error NotFound = new("CommonErrors.NotFound", "Patient role not found.", StatusCodes.Status404NotFound);
    public static readonly Error InvalidInput = new("CommonErrors.InvalidInput", "Invalid account status.", StatusCodes.Status404NotFound);
    public static readonly Error Unauthorized = new("CommonErrors.Unauthorized", "You are not authorized to delete this question.", StatusCodes.Status404NotFound);
   

}
