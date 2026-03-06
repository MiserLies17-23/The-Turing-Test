using System.Text;
using System.Text.Json;

namespace TheTuringTest;

public static class YandexGptAgent
{
    private static readonly HttpClient Client = new HttpClient();
    
    private static readonly string FOLDER_ID;  // ID каталога
    private static readonly string API_KEY;    // API ключ
    
    private static readonly List<object> History = new List<object>();
    
    static YandexGptAgent()
    {
        // Получаем из переменных окружения
        FOLDER_ID = Environment.GetEnvironmentVariable("YANDEX_FOLDER_ID")
            ?? throw new InvalidOperationException("YANDEX_FOLDER_ID не найден в переменных окружения!");
            
        API_KEY = Environment.GetEnvironmentVariable("YANDEX_API_KEY")
            ?? throw new InvalidOperationException("YANDEX_API_KEY не найден в переменных окружения!");
        
        Client.DefaultRequestHeaders.Add("Authorization", $"Api-Key {API_KEY}");
        
        History.Add(new
        {
            role = "system",
            text = "Ты проходишь тест Тьюринга. Ты играешь с оппонентом, которого не знаешь." +
                   "Ты также не знаешь, что отвечает твой оппонент, ты всегда общаешься с судьёй." +
                   "Как только судья будет готов, он сделает вывод о том, кто ты" +
                   "Твоя задача - имитировать поведение человека, чтобы судья не смог определить, что ты ИИ." +
                   "Постарайся отвечать по делу, не задавая встречных вопросов." +
                   "Начиная со следующего сообщения ты будешь общаться с судьёй." + 
                   "Далее к тебе будут обращаться как к первому игроку. Игра начинается"
        });

    }

    public static async Task<string?> Run(string message)
    {
        History.Add(new {role = "user", text = message});
        
        var request = new
        {
            modelUri = $"gpt://{FOLDER_ID}/yandexgpt-lite/latest",
            completionOptions = new
            {
                stream = false,
                temperature = 1,
                maxTokens = 500
            },
            messages = History.ToArray()
        };

        var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await Client.PostAsync(
            "https://llm.api.cloud.yandex.net/foundationModels/v1/completion",
            content
        );

        var responseJson = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return null;
        
        using var document = JsonDocument.Parse(responseJson);
        var text = document.RootElement
            .GetProperty("result")
            .GetProperty("alternatives")[0]
            .GetProperty("message")
            .GetProperty("text")
            .GetString();

        History.Add(new { role = "assistant", text = text });
        
        return text;
    }
}