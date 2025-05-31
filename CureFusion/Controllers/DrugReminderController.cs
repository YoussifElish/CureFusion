using CureFusion.Application.Contracts.Drug;
using CureFusion.Application.Services;

namespace CureFusion.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DrugReminderController(IDrugReminderService drugReminderService, ApplicationDbContext context) : ControllerBase
    {
        private readonly IDrugReminderService _drugReminderService = drugReminderService;
        private readonly ApplicationDbContext _context = context;
        [HttpPost("AddReminder")]
        public async Task<IActionResult> AddReminder(DrugReminderRequest drugReminderRequest)
        {
            var drug = await _context.Drugs.FirstOrDefaultAsync(d => d.Id == drugReminderRequest.DrugId);
            if (drug == null)
            {
                return BadRequest("Invalid drug ID.");
            }

            var reminder = drugReminderRequest.Adapt<DrugReminder>();
            reminder.Drug = drug;

            await _context.AddAsync(reminder);
            await _context.SaveChangesAsync();

            await _drugReminderService.ScheduleDrugReminderAsync(reminder);

            return Ok("Reminder scheduled.");
        }

    }
}
