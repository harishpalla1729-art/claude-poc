using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClaudeApiDemo
{
    class Program
    {
        private const string CLAUDE_API_URL = "https://api.anthropic.com/v1/messages";
        
        static async Task Main(string[] args)
        {
            Console.WriteLine("🤖 Claude API Demo - .NET Edition\n");
            
            string? apiKey = Environment.GetEnvironmentVariable("ANTHROPIC_API_KEY");
            
            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("❌ Error: ANTHROPIC_API_KEY environment variable not set!");
                Console.WriteLine("\nTo set it:");
                Console.WriteLine("  Windows: setx ANTHROPIC_API_KEY \"your-api-key-here\"");
                Console.WriteLine("  Mac/Linux: export ANTHROPIC_API_KEY=\"your-api-key-here\"");
                return;
            }

            try
            {
                await TestClaudeApi(apiKey);
                Console.WriteLine("\n✅ Success! Claude API is working correctly.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static async Task TestClaudeApi(string apiKey)
        {
            using var client = new HttpClient();
            
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

            var requestBody = new
            {
                model = "claude-sonnet-4-20250514",
                max_tokens = 1024,
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = "Say 'Hello from .NET!' and tell me a short programming joke."
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine("📤 Sending request to Claude...\n");
            
            var response = await client.PostAsync(CLAUDE_API_URL, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API Error: {response.StatusCode}\n{responseBody}");
            }

            using var jsonDoc = JsonDocument.Parse(responseBody);
            var root = jsonDoc.RootElement;
            
            if (root.TryGetProperty("content", out var contentArray) && 
                contentArray.GetArrayLength() > 0)
            {
                var textContent = contentArray[0].GetProperty("text").GetString();
                Console.WriteLine("📥 Claude's Response:");
                Console.WriteLine("─────────────────────");
                Console.WriteLine(textContent);
                Console.WriteLine("─────────────────────");
            }
        }
    }
}
