using CureFusion.Domain.Abstactions;
using Microsoft.AspNetCore.Http;



namespace CureFusion.Domain.Errors;

public static class DrugError
{
    public static readonly Error DrugNotFOund = new("DrugError.invalidCredentials", " drug not found ", StatusCodes.Status404NotFound);
    public static readonly Error Duplicatedrug = new("DrugError.Duplicateddrug", "this drug is already exsist", StatusCodes.Status406NotAcceptable);
}
