using Hangfire;

public class DrugReminderService : IDrugReminderService
{
    private readonly IBackgroundJobClient _backgroundJobClient;

   
    private readonly ILogger<DrugReminderService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IVoiceNotificationService _voiceNotificationService;

    public DrugReminderService(IBackgroundJobClient backgroundJobClient, ILogger<DrugReminderService> logger, ApplicationDbContext context, IVoiceNotificationService voiceNotificationService)
    {
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
        _context = context;
        _voiceNotificationService = voiceNotificationService;
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

        await _voiceNotificationService.SendVoiceNotificationAsync(
          $"201093441321",
          $"مرحبًا يوسف، نحن من فريق الدعم في موقع CureFusion. نود تذكيرك بأخذ الدواء الذي وصفه لك الطبيب، وهو {reminder.Drug.Name}. نرجو منك الالتزام بالجرعة المحددة في الوقت المحدد. مع تحياتنا، فريق CureFusion.",
          "ar", 
          1
      );



        _logger.LogInformation($"Sending reminder notification for drug {reminder.Drug.Name}");

        await Task.Delay(1000); 

        _logger.LogInformation($"Reminder sent for {reminder.Drug.Name} at {DateTime.Now}");
    }
}
