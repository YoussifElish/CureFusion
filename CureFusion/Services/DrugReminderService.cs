using Hangfire;
using Microsoft.AspNetCore.Identity;

public class DrugReminderService : IDrugReminderService
{
    private readonly IBackgroundJobClient _backgroundJobClient;


    private readonly ILogger<DrugReminderService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly ITwilioVoiceService _twilioVoiceService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DrugReminderService(IBackgroundJobClient backgroundJobClient, ILogger<DrugReminderService> logger, ApplicationDbContext context, ITwilioVoiceService twilioVoiceService, UserManager<ApplicationUser> userManager)
    {
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
        _context = context;
        _twilioVoiceService = twilioVoiceService;
        _userManager = userManager;
    }

    public async Task ScheduleDrugReminderAsync(DrugReminder reminder)
    {
        if (reminder.LastReminderTime == default)
        {
            reminder.LastReminderTime = reminder.StartDate;
            _logger.LogInformation($"Setting LastReminderTime to StartDate: {reminder.StartDate}");
        }

        var nextReminderTime = reminder.LastReminderTime.AddMinutes(reminder.RepeatIntervalMinutes);

        if (nextReminderTime >= reminder.StartDate && nextReminderTime <= reminder.EndDate)
        {
            _logger.LogInformation($"Scheduling reminder for drug {reminder.Drug.Name} at {nextReminderTime}");

            _backgroundJobClient.Schedule(() => SendReminderNotification(reminder), nextReminderTime);

            reminder.LastReminderTime = nextReminderTime;

            _context.Update(reminder);
            await _context.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation($"Reminder for drug {reminder.Drug.Name} is outside the reminder period.");
        }
    }

    public async Task SendReminderNotification(DrugReminder reminder)
    {

        var user = await _userManager.FindByIdAsync(reminder.UserId);

        if (user == null)
        {
            _logger.LogError("User not found.");
            return;
        }
        _logger.LogInformation($"Sending reminder notification for drug {reminder.Drug.Name}");

        //      var result = await _twilioVoiceService.MakeVoiceCallAsync(
        //    toPhoneNumber: "+201093441321",
        //    message: $"مرحبًا {user.FirstName}، نحن من فريق الدعم في موقع CureFusion. نود تذكيرك بأخذ الدواء الذي وصفه لك الطبيب، وهو {reminder.Drug.Name}. نرجو منك الالتزام بالجرعة المحددة في الوقت المحدد. مع تحياتنا، فريق CureFusion.",
        //    language: "ar-AE",
        //    voice: "Polly.Hala-Neural"
        //);



        //      _logger.LogInformation($"Reminder sent for {reminder.Drug.Name} at {DateTime.Now} account sid : {result.AccountSid} Ended At {result.EndTime} Duration : {result.Duration} QueueTime : {result.QueueTime}  URI : {result.Uri}");
    }
}
