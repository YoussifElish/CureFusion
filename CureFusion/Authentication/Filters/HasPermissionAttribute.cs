using Microsoft.AspNetCore.Authorization;

namespace CureFusion.Authentication.Filters
{
    public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
    {

    }
}
