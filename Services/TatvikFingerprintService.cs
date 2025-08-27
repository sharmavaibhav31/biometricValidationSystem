using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace FingerprintService.Services
{
    public interface ITatvikFingerprintService
    {
        bool IsDeviceConnected();
        object GetDeviceInfo();
        byte[] CaptureTemplate(int timeoutMs);
        bool MatchIsoTemplates(byte[] referenceTemplate, byte[] claimedTemplate);
    }

    // Adapter client that calls local .NET Framework SDK bridge over HTTP
    public class TatvikFingerprintService : ITatvikFingerprintService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public TatvikFingerprintService()
        {
            _httpClient = new HttpClient();
            _baseUrl = Environment.GetEnvironmentVariable("TMF20_BRIDGE_URL") ?? "http://127.0.0.1:5010";
        }

        public bool IsDeviceConnected()
        {
            var resp = _httpClient.GetAsync($"{_baseUrl}/device/check").GetAwaiter().GetResult();
            resp.EnsureSuccessStatusCode();
            var json = resp.Content.ReadFromJsonAsync<JsonElement>().GetAwaiter().GetResult();
            return json.GetProperty("connected").GetBoolean();
        }

        public object GetDeviceInfo()
        {
            var resp = _httpClient.GetAsync($"{_baseUrl}/device/info").GetAwaiter().GetResult();
            resp.EnsureSuccessStatusCode();
            var json = resp.Content.ReadFromJsonAsync<JsonElement>().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<object>(json.GetRawText());
        }

        public byte[] CaptureTemplate(int timeoutMs)
        {
            var resp = _httpClient.GetAsync($"{_baseUrl}/fingerprint/capture?timeoutMs={timeoutMs}").GetAwaiter().GetResult();
            resp.EnsureSuccessStatusCode();
            var json = resp.Content.ReadFromJsonAsync<JsonElement>().GetAwaiter().GetResult();
            var base64 = json.GetProperty("template").GetString();
            return Convert.FromBase64String(base64);
        }

        public bool MatchIsoTemplates(byte[] referenceTemplate, byte[] claimedTemplate)
        {
            var body = new
            {
                ReferenceTemplate = Convert.ToBase64String(referenceTemplate),
                ClaimedTemplate = Convert.ToBase64String(claimedTemplate)
            };
            var resp = _httpClient.PostAsync($"{_baseUrl}/fingerprint/match",
                new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
            resp.EnsureSuccessStatusCode();
            var json = resp.Content.ReadFromJsonAsync<JsonElement>().GetAwaiter().GetResult();
            return json.GetProperty("matched").GetBoolean();
        }
    }
}


