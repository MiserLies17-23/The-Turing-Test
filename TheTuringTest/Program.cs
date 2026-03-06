namespace TheTuringTest;

public static class Program
{
    public static async Task Main()
    {
        Ui ui = new Ui();
        await ui.Run();
    }
}