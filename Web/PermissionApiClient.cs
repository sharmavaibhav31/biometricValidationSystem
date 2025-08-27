using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FingerprintService.Web
{
    public interface IPermissionApiClient
    {
        Task<PermissionDecision> CheckAndLogAsync(string studentId, string direction);
    }

    public class PermissionDecision
    {
        public string StudentId { get; set; }
        public string PermissionStatus { get; set; }
        public bool Logged { get; set; }
        public string Message { get; set; }
        public DateTime TimestampUtc { get; set; }
    }

    public class PermissionApiClient : IPermissionApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public PermissionApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["PermissionApi:BaseUrl"] ?? "http://localhost:8080";
            _apiKey = configuration["PermissionApi:ApiKey"] ?? string.Empty;
        }

        public async Task<PermissionDecision> CheckAndLogAsync(string studentId, string direction)
        {
            var payload = new { studentId, direction };
            var req = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl.TrimEnd('/')}/api/permissions/check-and-log")
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };
            if (!string.IsNullOrEmpty(_apiKey))
            {
                req.Headers.Add("X-Api-Key", _apiKey);
            }
            var resp = await _httpClient.SendAsync(req);
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadFromJsonAsync<PermissionDecision>();
            return json ?? new PermissionDecision
            {
                StudentId = studentId,
                PermissionStatus = "Unknown",
                Logged = false,
                Message = "Empty response",
                TimestampUtc = DateTime.UtcNow
            };
        }
    }
}


