namespace CureFusion.API.Authentication.Filters
{
    public class PermissionRequirment(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }
}
