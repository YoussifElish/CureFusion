
namespace CureFusion.Services;

public class UserService(UserManager<ApplicationUser> user, ApplicationDbContext context) : IUserService
{
    private readonly UserManager<ApplicationUser> _user = user;
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<UserResponse>> AddAsync(UserRequest request, CancellationToken cancellationToken)
    {
        var isexsistuser = await _user.FindByEmailAsync(request.Email);
        if (isexsistuser is not null)
            return Result.Failure<UserResponse>(AuthErrors.DuplicatedEmail);


        var Allowedroles =
            new List<string> { DefaultRoles.Admin, DefaultRoles.Member, DefaultRoles.Doctor };
        var Invalidrole = request.Roles.Except(Allowedroles).ToList();

        if (Invalidrole.Any())
            return Result.Failure<UserResponse>(AuthErrors.Invalidroles);

        var User = request.Adapt<ApplicationUser>();
        var result = await _user.CreateAsync(User, request.Password);

        if (!result.Succeeded) {
            var error = result.Errors.First();
            return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }

        await _user.AddToRolesAsync(User, request.Roles);

        await _context.SaveChangesAsync(cancellationToken);
        var response = (User, request.Roles).Adapt<UserResponse>();

        return Result.Success(response);


    }

    public async Task<Result<PageinatedList<UserResponse>>> GetAllUsersAsync(UserQueryParameters userQuery, CancellationToken cancellationToken)
    {
        var query =  from u in _context.Users
                           join ur in _context.UserRoles
                           on u.Id equals ur.UserId
                           join r in _context.Roles
                           on ur.RoleId equals r.Id into roles
                           where !roles.Any(x => x.Name == DefaultRoles.Member) //comment it lw 3ayz kol alusers
                           select new
                           {
                               u.Id,
                               u.FirstName,
                               u.LastName,
                               u.Email,
                               u.IsDisabled,
                               roles = roles.Select(r => r.Name!)
                           };
                           
                         if (!string.IsNullOrEmpty(userQuery.SearchTerm))
        {
            query = query.Where(x =>
                x.FirstName.Contains(userQuery.SearchTerm) ||
                x.LastName.Contains(userQuery.SearchTerm) ||
                x.Email.Contains(userQuery.SearchTerm)
            );
        }
        query = userQuery.SortBy?.ToLower() switch
        {
            "firstname" => userQuery.SortAscending ? query.OrderBy(x => x.FirstName) : query.OrderByDescending(x => x.FirstName),
            "lastname" => userQuery.SortAscending ? query.OrderBy(x => x.LastName) : query.OrderByDescending(x => x.LastName),
            "email" => userQuery.SortAscending ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email),
            _ => userQuery.SortAscending ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id)
        };
        var totalCount = await query.CountAsync(cancellationToken);
        var users = await query
       .Skip((userQuery.PageNumber - 1) * userQuery.PageSize)
       .Take(userQuery.PageSize)
       .GroupBy(x => new
       {
           x.Id,
           x.FirstName,
           x.LastName,
           x.Email,
           x.IsDisabled
       })

                       .Select(g => new UserResponse
                        (
                            g.Key.Id, 
                            g.Key.FirstName,
                            g.Key.LastName,
                            g.Key.Email,
                            g.Key.IsDisabled,
                            g.SelectMany(x => x.roles)
                        ))

                        .ToListAsync(cancellationToken);
        var paginatedResult = new PageinatedList<UserResponse>(users,totalCount,userQuery.PageNumber,userQuery.PageSize
        );
        return Result.Success(paginatedResult);


    }

    public async Task<Result<UserResponse>> GetUsersAsync(string Id, CancellationToken cancellationToken)
    {
     var user= await _user.FindByIdAsync(Id);
        if (user is null)
            return Result.Failure<UserResponse>(AuthErrors.NotFound);
        var roles = await _user.GetRolesAsync(user);
        var response=(user,roles).Adapt<UserResponse>();
        return Result.Success<UserResponse>(response);
    
    }

    public async  Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken)
    {
        var user = await _user.FindByIdAsync(id);
        if (user is null)
            return Result.Failure(AuthErrors.NotFound);
        user.IsDisabled = !user.IsDisabled;
        var result = await _user.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UnlockUserAsync(string id, CancellationToken cancellationToken)
    {
        var user = await _user.FindByIdAsync(id);
        if (user is null)
            return Result.Failure(AuthErrors.NotFound);
        user.IsDisabled = !user.IsDisabled;
        var result = await _user.SetLockoutEndDateAsync(user, null);
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> UpdateAsync(string id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var isexsistuser = await _user.Users.AnyAsync(x=>x.Email==request.Email&&x.Id!=id,cancellationToken);
        if (isexsistuser)
            return Result.Failure(AuthErrors.DuplicatedEmail);


        var Allowedroles =
            new List<string> { DefaultRoles.Admin, DefaultRoles.Member, DefaultRoles.Doctor };
        var Invalidrole = request.Roles.Except(Allowedroles).ToList();

        if (Invalidrole.Any())
            return Result.Failure(AuthErrors.Invalidroles);

        var User = await _user.FindByIdAsync(id);
        if (User is null)
            return Result.Failure(AuthErrors.NotFound);

        User = request.Adapt(User); //same logic as x+=x+1

        var result = await _user.UpdateAsync(User);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }
        await _context.UserRoles
            .Where(x => x.UserId ==User.Id)
            .ExecuteDeleteAsync(cancellationToken);

        await _user.AddToRolesAsync(User, request.Roles);

        await _context.SaveChangesAsync(cancellationToken);
      
        return Result.Success();
    }
}
