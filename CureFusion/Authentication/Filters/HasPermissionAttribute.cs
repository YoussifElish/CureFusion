namespace CureFusion.API.Authentication.Filters
{
    public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
    {

    }
}
