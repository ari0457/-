using System;

interface ILogger<T>
{
    void Log(T message);
}

class ConsoleLogger<T> : ILogger<T>
{
    public void Log(T message)
    {
        Console.WriteLine($"[INFO] {message}");
    }
}

class LoggerManager<T>
{
    private ILogger<T> logger;

    public LoggerManager(ILogger<T> logger)
    {
        this.logger = logger;
    }

    public void LogError(T message)
    {
        Console.WriteLine($"[ERROR] {message}");
    }

    public void LogWarning(T message)
    {
        Console.WriteLine($"[WARNING] {message}");
    }

    public void LogInfo(T message)
    {
        logger.Log(message);
    }
}

class Program
{
    static void Main()
    {
        ConsoleLogger<string> consoleLogger = new ConsoleLogger<string>();
        LoggerManager<string> manager = new LoggerManager<string>(consoleLogger);

        manager.LogInfo("Программа запущена");
        manager.LogWarning("Низкий заряд батареи");
        manager.LogError("Не удалось подключиться к базе данных");

        Console.WriteLine();

        LoggerManager<int> intManager = new LoggerManager<int>(new ConsoleLogger<int>());
        intManager.LogInfo(100);
        intManager.LogWarning(200);
        intManager.LogError(500);
    }
}