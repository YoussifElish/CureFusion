using CureFusion.Settings;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace CureFusion.Services;

public class TwilioVoiceService : ITwilioVoiceService
{
    private readonly TwilioSettings _settings;

    public TwilioVoiceService(IOptions<TwilioSettings> settings)
    {
        _settings = settings.Value;
        TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
    }
    public async Task<CallResource> MakeVoiceCallAsync(string toPhoneNumber, string message, string language, string voice)
    {
        var response = new Twilio.TwiML.VoiceResponse();
        response.Say(message, language: language, voice: voice);

        var twiml = response.ToString();

        var call = await CallResource.CreateAsync(
            to: new PhoneNumber(toPhoneNumber),
            from: new PhoneNumber(_settings.FromPhoneNumber),
            twiml: new Twiml(twiml)
        );

        return call;
    }
}