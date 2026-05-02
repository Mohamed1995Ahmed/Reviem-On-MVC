using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace WebApplication2.Controllers
{
	public class AiChatpotController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly IConfiguration _configuration;

		public AiChatpotController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
		{
			_httpClientFactory = httpClientFactory;
			_configuration = configuration;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
		{
			if (string.IsNullOrWhiteSpace(request?.Message))
				return BadRequest(new { error = "Message cannot be empty." });

			var apiKey = _configuration["Gemini:ApiKey"];
			if (string.IsNullOrEmpty(apiKey))
				return StatusCode(500, new { error = "Gemini API key is not configured." });

			var client = _httpClientFactory.CreateClient();

			// Build conversation history for Gemini
			var contents = new List<object>();

			if (request.History != null)
			{
				foreach (var h in request.History)
				{
					contents.Add(new
					{
						role = h.Role == "assistant" ? "model" : "user",
						parts = new[] { new { text = h.Content } }
					});
				}
			}

			// Add current user message
			contents.Add(new
			{
				role = "user",
				parts = new[] { new { text = request.Message } }
			});

			var payload = new
			{
				system_instruction = new
				{
					parts = new[] { new { text = "You are a helpful, friendly, and concise AI assistant. Respond clearly and conversationally." } }
				},
				contents
			};

			var json = JsonSerializer.Serialize(payload);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";
			var response = await client.PostAsync(url, content);

			if (!response.IsSuccessStatusCode)
			{
				var errorBody = await response.Content.ReadAsStringAsync();
				return StatusCode((int)response.StatusCode, new { error = errorBody });
			}

			var responseBody = await response.Content.ReadAsStringAsync();
			using var doc = JsonDocument.Parse(responseBody);
			var replyText = doc.RootElement
				.GetProperty("candidates")[0]
				.GetProperty("content")
				.GetProperty("parts")[0]
				.GetProperty("text")
				.GetString();

			return Ok(new { reply = replyText });
		}
	}

	public class ChatRequest
	{
		public string Message { get; set; } = "";
		public List<ChatHistoryItem>? History { get; set; }
	}

	public class ChatHistoryItem
	{
		public string Role { get; set; } = "";
		public string Content { get; set; } = "";
	}
}