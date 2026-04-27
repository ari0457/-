using System;
using System.IO;

class FileWatcher
{
    private FileSystemWatcher watcher;
    private string watchPath;

    public FileWatcher(string path)
    {
        watchPath = path;
        watcher = new FileSystemWatcher(path);
        watcher.Created += OnCreated;
        watcher.Deleted += OnDeleted;
        watcher.Changed += OnChanged;
        watcher.Renamed += OnRenamed;
        watcher.EnableRaisingEvents = true;
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"[СОЗДАН] {e.FullPath} в {DateTime.Now}");
        SendEmailNotification($"Создан новый файл: {e.Name}");
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"[УДАЛЕН] {e.FullPath} в {DateTime.Now}");
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"[ИЗМЕНЕН] {e.FullPath} в {DateTime.Now}");
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        Console.WriteLine($"[ПЕРЕИМЕНОВАН] {e.OldFullPath} -> {e.FullPath} в {DateTime.Now}");
    }

    private void SendEmailNotification(string message)
    {
        Console.WriteLine($"\n[EMAIL-УВЕДОМЛЕНИЕ] {message}");
        Console.WriteLine($"Отправлено на: admin@example.com в {DateTime.Now}\n");
    }

    public void StopWatching()
    {
        watcher.EnableRaisingEvents = false;
        watcher.Dispose();
        Console.WriteLine("Отслеживание остановлено");
    }
}

class Program
{
    static void Main()
    {
        string watchDirectory = AppDomain.CurrentDomain.BaseDirectory;
        Console.WriteLine($"Отслеживание директории: {watchDirectory}");
        Console.WriteLine("Для остановки нажмите любую клавишу...\n");

        FileWatcher watcher = new FileWatcher(watchDirectory);

        Console.WriteLine("Попробуйте создать, изменить или удалить файл в этой папке");

        Console.ReadKey();
        watcher.StopWatching();
    }
}