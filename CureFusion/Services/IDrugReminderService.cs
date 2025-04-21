namespace CureFusion.Services;

public interface IDrugReminderService
{
    Task ScheduleDrugReminderAsync(DrugReminder reminder);
}
