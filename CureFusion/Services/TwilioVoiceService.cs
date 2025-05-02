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
    }
    public async Task<CallResource> MakeVoiceCallAsync(string toPhoneNumber, string message, string language, string voice)
    {
        TwilioClient.Init("ACf171a5e75606504fddb7e371a397ce93", "09926345f47344f6452ad640fbb0b713");

        var response = new Twilio.TwiML.VoiceResponse();
        response.Say(message, language: language, voice: voice);

        var twiml = response.ToString();

        var call = await CallResource.CreateAsync(
            to: new PhoneNumber(toPhoneNumber),
            from: new PhoneNumber("+13046895041"),
            twiml: new Twiml(twiml)
        );

        return call;
    }
}