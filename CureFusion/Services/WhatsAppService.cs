namespace CureFusion.Services;

using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using CureFusion.Settings;

public class WhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly WhatsAppConfigurations _config;

    public WhatsAppService(IOptions<WhatsAppConfigurations> config)
    {
        _httpClient = new HttpClient();
        _config = config.Value;
    }
    public async Task SendTemplateMessageAsync(string toPhoneNumber, string templateName, string languageCode, List<string> parameters)
    {
        var url = $"https://graph.facebook.com/v18.0/{_config.PhoneNumberId}/messages";

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _config.AccessToken);

        var bodyParams = parameters.Select(p => new
        {
            type = "text",
            text = p
        }).ToArray();

        var payload = new
        {
            messaging_product = "whatsapp",
            to = toPhoneNumber,
            type = "template",
            template = new
            {
                name = templateName,
                language = new { code = languageCode },
                components = new[]
                {
                new
                {
                    type = "body",
                    parameters = bodyParams
                }
            }
            }
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode(); // Handle or log errors as needed
    }

}
