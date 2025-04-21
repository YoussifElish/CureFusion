namespace CureFusion.Contracts.Drug;

public record DrugReminderRequest
    (
        string UserId,   
        int DrugId,      
        DateTime StartDate,  
        DateTime EndDate,  
        int RepeatIntervalMinutes  
    );