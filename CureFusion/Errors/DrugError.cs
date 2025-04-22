using CureFusion.Abstactions;
using Vonage.Voice.EventWebhooks;
using Error = CureFusion.Abstactions.Error;


namespace CureFusion.Errors;

public static class DrugError
{
    public static readonly Error DrugNotFOund = new("DrugError.invalidCredentials", " drug not found ", StatusCodes.Status404NotFound);
    public static readonly Error Duplicatedrug = new("DrugError.Duplicateddrug", "this drug is already exsist", StatusCodes.Status406NotAcceptable);
}
