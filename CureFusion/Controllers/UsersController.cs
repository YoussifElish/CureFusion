

namespace CureFusion.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService user) : ControllerBase
{
    private readonly IUserService _user = user;


    [HttpGet("GetAll")]
    [Authorize(Roles = $"{DefaultRoles.Admin}")]
    public async Task<IActionResult> GetAllUsersAsync([FromQuery] UserQueryParameters userQuery,CancellationToken cancellationToken)
    {
        var users = await _user.GetAllUsersAsync(userQuery,cancellationToken);
        return Ok(users);
    }

    [HttpGet("Get/{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin}")]
    public async Task<IActionResult> GetUserAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var user = await _user.GetUsersAsync(id, cancellationToken);
        return user.IsSuccess
            ? Ok(user.Value)
            : user.ToProblem();
    }

    [HttpPost("Add")]
    [Authorize(Roles = $"{DefaultRoles.Admin}")]
    public async Task<IActionResult> AddUserAsync([FromBody] UserRequest request, CancellationToken cancellationToken)
    {
        var user = await _user.AddAsync(request, cancellationToken);
        return user.IsSuccess
            ? Ok( user.Value)
            : user.ToProblem();
    }

    [HttpPut("Update/{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin}")]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] string id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _user.UpdateAsync(id, request, cancellationToken);
        return user.IsSuccess
            ? NoContent()
            : user.ToProblem();
    }

    [HttpPut("ToggleStatus/{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin}")]
    public async Task<IActionResult> ToggleStatusAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var user = await _user.ToggleStatusAsync(id, cancellationToken);
        return user.IsSuccess
            ? NoContent()
            : user.ToProblem();
    }

    [HttpPut("UnlockUser/{id}")]
    [Authorize(Roles = $"{DefaultRoles.Admin}")]
    public async Task<IActionResult> UnlockUserAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var user = await _user.UnlockUserAsync(id, cancellationToken);
        return user.IsSuccess
            ? NoContent()
            : user.ToProblem();
    }

}
