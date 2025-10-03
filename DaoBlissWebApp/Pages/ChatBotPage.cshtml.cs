using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DaoBlissWebApp.Pages
{
	public class ChatBotPage : PageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IWebHostEnvironment _env;
		private readonly ILogger<ChatBotPage> _logger;
		private readonly string _apiKey;

		public ChatBotPage(IHttpClientFactory httpClientFactory, IWebHostEnvironment env, ILogger<ChatBotPage> logger, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_env = env;
			_logger = logger;
			_apiKey = configuration["Gemini:ApiKey"] ?? throw new InvalidOperationException("Gemini API key not configured.");
		}

		public void OnGet()
		{
			_logger.LogInformation("OnGet called");
		}
		public class RequestFrom
		{
			public List<ChatMessage> ChatHistory { get; set; } = new();
		}

		public class ChatMessage
		{
			public string Role { get; set; }
			public List<ChatPart> Parts { get; set; } = new();
		}

		public class ChatPart
		{
			public string Text { get; set; }
		}

		public async Task<IActionResult> OnPostSendMessageAsync([FromBody] RequestFrom request)
		{
			if (request?.ChatHistory == null || request.ChatHistory.Count == 0 || string.IsNullOrWhiteSpace(request.ChatHistory[^1].Parts[0].Text))
				return new JsonResult(new { error = new { message = "Invalid message." } }) { StatusCode = 400 };

			try
			{
				var filePath = Path.Combine(_env.WebRootPath, "chatbot", "data.txt");
				var chatBotModel = Path.Combine(_env.WebRootPath, "chatbot", "chatbotmodel.txt");
				string context = System.IO.File.Exists(filePath) ? await System.IO.File.ReadAllTextAsync(filePath) : "";
				string model = System.IO.File.Exists(chatBotModel) ? await System.IO.File.ReadAllTextAsync(chatBotModel) : "";
				string systemInstruction = context;

				var client = _httpClientFactory.CreateClient("Gemini");
				var payload = new
				{
					contents = new[]{
						new
				{
						role = "user", 
						parts = new[] { new { text = systemInstruction } }
				}}
				.Concat(request.ChatHistory.TakeLast(3).Select(msg => new{
					role = msg.Role,
					parts = msg.Parts.Select(p => new { text = p.Text }).ToArray()})
				).ToArray()
				};

				var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
				var response = await client.PostAsync($"v1beta/models/{model}:generateContent?key={_apiKey}", content);

				if (!response.IsSuccessStatusCode)
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					_logger.LogError($"Gemini API error: {response.StatusCode} - {errorContent}");
					return new JsonResult(new { error = new { message = $"API request failed: {errorContent}" } }) { StatusCode = 500 };
				}

				var json = await response.Content.ReadAsStringAsync();
				using var doc = JsonDocument.Parse(json);
				if (!doc.RootElement.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
					return new JsonResult(new { error = new { message = "Invalid API response format." } }) { StatusCode = 500 };

				string replyText = candidates[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString() ?? "Không tìm thấy thông tin liên quan.";
				var result = new
				{
					candidates = new[] { new { content = new { parts = new[] { new { text = replyText } } } } }
				};

				return new JsonResult(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error.");
				return new JsonResult(new { error = new { message = "Unexpected error occurred." } }) { StatusCode = 500 };
			}
		}
	}
}