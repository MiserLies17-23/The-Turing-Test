namespace TheTuringTest;

public class Ui
{
    public async Task Run()
    {
        Console.WriteLine("Начало общения...");

        try
        {
            Console.WriteLine("=== Тест Тьюринга (демо) ===");
            Console.WriteLine("Для выхода введите 'exit'");
            Console.WriteLine();
        
            while (true)
            {
                Console.Write("Вы: ");
                var input = Console.ReadLine();
            
                if (input?.ToLower() == "exit") break;
            
                Console.Write("YandexGPT: ");
                var response = await YandexGptAgent.Run(input!);
                Console.WriteLine(response ?? "Не удалось получить ответ");
                Console.WriteLine();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ошибка: {e.Message}");
        }
    }
}