using CureFusion.Settings;
using Microsoft.Extensions.Options;
using Vonage;
using Vonage.Request;
using Vonage.Voice;
using Vonage.Voice.Nccos;
using Vonage.Voice.Nccos.Endpoints;

public class VoiceNotificationService : IVoiceNotificationService
{
    private readonly VonageSettings _settings;
    private readonly VonageClient _vonageClient;

    public VoiceNotificationService(IOptions<VonageSettings> options)
    {
        _settings = options.Value;
        var credentials = Credentials.FromAppIdAndPrivateKeyPath(
    _settings.ApplicationId,
    _settings.PrivateKeyPath
);

        _vonageClient = new VonageClient(credentials);
    }

    public async Task<string> SendVoiceNotificationAsync(string toNumber, string message, string language = "en-US", int style = 0)
    {
        var toEndpoint = new PhoneEndpoint { Number = toNumber };
        var fromEndpoint = new PhoneEndpoint { Number = _settings.FromNumber };

        var talkAction = new TalkAction
        {

            Text = message,
            Language = language,
            Style = style
        };

        var ncco = new Ncco(talkAction);

        var callCommand = new CallCommand
        {
            To = new[] { toEndpoint },
            From = fromEndpoint,
            Ncco = ncco
        };

        var response = await _vonageClient.VoiceClient.CreateCallAsync(callCommand);
        return response.Uuid;
    }
}
