using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BusinessLogicLayer.DTOs.Teacher.Schedules;
using BusinessLogicLayer.Services.Interfaces.Teacher.Schedules;
using Microsoft.Extensions.Configuration;

namespace BusinessLogicLayer.Services.Teacher.Schedules;

public class VoiceScheduleAnalyzer : IVoiceScheduleAnalyzer
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public VoiceScheduleAnalyzer(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClient = httpClientFactory.CreateClient("GeminiClient");
        _apiKey = config["GeminiSettings:ApiKey"] ?? string.Empty;
        _model = config["GeminiSettings:Model"] ?? "gemini-1.5-flash";
    }

    public async Task<CreateScheduleDto> ParseTranscriptAsync(string transcript)
    {
        // Default fallback
        var defaultDto = new CreateScheduleDto
        {
            Title = "AI Generated Class",
            StartTime = DateTime.Today.AddHours(9),
            EndTime = DateTime.Today.AddHours(11),
            IsTrial = false
        };

        if (string.IsNullOrWhiteSpace(_apiKey) || _apiKey == "YOUR_API_KEY")
        {
            throw new Exception("Chưa cấu hình GeminiSettings:ApiKey trong appsettings.json. Hệ thống không thể gọi AI API.");
        }

        var systemPrompt = $@"
Bạn là một trợ lý AI phân tích ngôn ngữ tự nhiên. 
Hôm nay là: {DateTime.Now:yyyy-MM-dd HH:mm:ss} (Theo giờ Vietnam).
Nhiệm vụ của bạn là đọc một chuỗi transcript (ngôn ngữ tự nhiên) mà người dùng nói ra, và trích xuất thông tin để tạo lịch học (Schedule).
Bạn phải trả về DUY NHẤT một chuỗi JSON chuẩn (không có markdown code block như ```json).
Các trường cần trả về (đúng tên):
- Title (string): Tên khoá học/tiêu đề lịch học được đề cập (Ví dụ: Chữa đề Toán 1). Format in hoa chữ cái đầu.
- StartTime (string, định dạng yyyy-MM-ddTHH:mm:00): Thời gian bắt đầu. (Nếu chỉ nói ngày, lấy mặc định 09:00:00).
- EndTime (string, định dạng yyyy-MM-ddTHH:mm:00): Thời gian kết thúc. (Nếu không nói rõ lượng thời gian, lấy mặc định +2 tiếng so với StartTime).
- IsTrial (boolean): Chuyển thành true nếu người dùng có nói là 'lớp học thử', 'trial', 'buổi thử'. Trái lại là false.

Ví dụ: Transcript là 'lên lịch buổi dạy toán 12 vào 7 giờ tối ngày mai kéo dài đến 9 rưỡi tối'
Bởi vì hôm nay là {DateTime.Now:yyyy-MM-dd}, ngày mai là {DateTime.Now.AddDays(1):yyyy-MM-dd}.
Phải trả về:
{{
  ""Title"": ""Buổi dạy Toán 12"",
  ""StartTime"": ""{DateTime.Now.AddDays(1):yyyy-MM-dd}T19:00:00"",
  ""EndTime"": ""{DateTime.Now.AddDays(1):yyyy-MM-dd}T21:30:00"",
  ""IsTrial"": false
}}
";

        var payload = new
        {
            systemInstruction = new { parts = new[] { new { text = systemPrompt } } },
            contents = new[]
            {
                new { parts = new[] { new { text = transcript } } }
            },
            generationConfig = new
            {
                responseMimeType = "application/json",
                temperature = 0.0
            }
        };

        var requestContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var requestUri = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";
        
        using var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        requestMessage.Content = requestContent;

        try
        {
            var response = await _httpClient.SendAsync(requestMessage);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini API Error: {response.StatusCode} - {content}");
            }

            using var document = JsonDocument.Parse(content);
            var resultText = document.RootElement
                                     .GetProperty("candidates")[0]
                                     .GetProperty("content")
                                     .GetProperty("parts")[0]
                                     .GetProperty("text")
                                     .GetString()?.Trim();

            if (!string.IsNullOrEmpty(resultText))
            {
                var aiResult = JsonSerializer.Deserialize<CreateScheduleDto>(resultText);
                if (aiResult != null)
                {
                    return aiResult;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Lỗi kết nối Gemini API: {ex.Message}");
        }

        return defaultDto;
    }
}
