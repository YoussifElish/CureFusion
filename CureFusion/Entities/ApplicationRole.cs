using Microsoft.AspNetCore.Identity;

namespace CureFusion.Entities;

public class ApplicationRole : IdentityRole
{
    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; }
}
