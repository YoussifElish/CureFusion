using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CureFusion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController(ISessionService sessionService) : ControllerBase
    {
        private readonly ISessionService _sessionService = sessionService;
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllSessions(CancellationToken cancellationToken)
        {
            var result = await _sessionService.GetAllSessionsAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("terminate")]
        [Authorize]

        public async Task<IActionResult> TerminateSession([FromQuery] int sessionId, CancellationToken cancellationToken)
        {
            var result = await _sessionService.TerminateSessionAsync(sessionId, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPost("terminate-all")]
        [Authorize]

        public async Task<IActionResult> TerminateAllSessions(CancellationToken cancellationToken)
        {
            var result = await _sessionService.TerminateAllSessionsAsync(cancellationToken);
            return result.IsSuccess ? Ok(new { terminated = result.Value }) : result.ToProblem();
        }
    
}
}
