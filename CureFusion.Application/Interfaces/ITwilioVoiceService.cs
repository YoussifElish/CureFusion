
using Twilio.Rest.Api.V2010.Account;

namespace CureFusion.Application.Services;

public interface ITwilioVoiceService
{
    Task<CallResource> MakeVoiceCallAsync(string toPhoneNumber, string message, string language, string voice);

}
