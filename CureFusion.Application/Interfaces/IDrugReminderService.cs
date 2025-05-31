using CureFusion.Domain.Entities;

namespace CureFusion.Application.Services;

public interface IDrugReminderService
{
    Task ScheduleDrugReminderAsync(DrugReminder reminder);
}
