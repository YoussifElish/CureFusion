namespace CureFusion.Services;

public interface IVoiceNotificationService
{
    Task<string> SendVoiceNotificationAsync(string toNumber, string message, string language = "en-US", int style = 0);

}
