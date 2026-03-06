using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace TheTuringTest;

public static class AiAgent
{
    private static readonly HttpClient Client = new HttpClient();
    
    private const string API_KEY = "KEY";

    public static async Task<string?> Run(string message)
    {
        Client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", API_KEY);
        
        object request = new
        {
            model = "gpt-3.5-turbo",  // Бесплатная модель
            messages = message,
            temperature = 0.7,
            max_tokens = 500,
            top_p = 0.9,
            frequency_penalty = 0,
            presence_penalty = 0
        };

        string? response = await AiResponse(request);
        return response;
    }

    private static async Task<string?> AiResponse(object request)
    {
        var json = JsonSerializer.Serialize(request, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
        });
        
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        // Ждем ответ от сервера 
        var response = await Client.PostAsync(
            "https://api.openai.com/v1/chat/completions",
            content
        );
        
        // Проверяем успешность запроса
        response.EnsureSuccessStatusCode();
        
        // Ждем чтение ответа (обязательно await!)
        var responseJson = await response.Content.ReadAsStringAsync();
        
        // Парсим JSON
        using var document = JsonDocument.Parse(responseJson);
        var root = document.RootElement;
        
        // Извлекаем текст
        var text = root
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
            
        return text;
    }
}