using System;
using System.IO;

public class FileChangedEventArgs : EventArgs
{
    public string FileName { get; set; }
    public string ChangeType { get; set; }
    public DateTime ChangeTime { get; set; }

    public FileChangedEventArgs(string name, string type)
    {
        FileName = name;
        ChangeType = type;
        ChangeTime = DateTime.Now;
    }
}

class FileWatcher
{
    public event EventHandler<FileChangedEventArgs> FileChanged;

    public void ChangeFile(string fileName)
    {
        FileChanged?.Invoke(this, new FileChangedEventArgs(fileName, "Изменен"));
    }

    public void DeleteFile(string fileName)
    {
        FileChanged?.Invoke(this, new FileChangedEventArgs(fileName, "Удален"));
    }

    public void CreateFile(string fileName)
    {
        FileChanged?.Invoke(this, new FileChangedEventArgs(fileName, "Создан"));
    }
}

class BackupService
{
    public void OnFileChanged(object sender, FileChangedEventArgs e)
    {
        string backupFile = $"backup_{e.FileName}_{DateTime.Now:HHmmss}";
        Console.WriteLine($"Бэкап: Создана копия файла '{e.FileName}' как '{backupFile}'");
        File.WriteAllText(backupFile, $"Бэкап файла {e.FileName} от {e.ChangeTime}");
    }
}

class Logger
{
    public void OnFileChanged(object sender, FileChangedEventArgs e)
    {
        string logEntry = $"[{e.ChangeTime}] Файл '{e.FileName}' - {e.ChangeType}";
        Console.WriteLine($"Логгер: {logEntry}");
        File.AppendAllText("file_log.txt", logEntry + Environment.NewLine);
    }
}

class FileMonitor
{
    private FileWatcher watcher;
    private BackupService backup;
    private Logger logger;

    public FileMonitor()
    {
        watcher = new FileWatcher();
        backup = new BackupService();
        logger = new Logger();

        Subscribe();
    }

    private void Subscribe()
    {
        watcher.FileChanged += backup.OnFileChanged;
        watcher.FileChanged += logger.OnFileChanged;
        Console.WriteLine("Монитор: Подписка оформлена\n");
    }

    public void Unsubscribe()
    {
        watcher.FileChanged -= backup.OnFileChanged;
        watcher.FileChanged -= logger.OnFileChanged;
        Console.WriteLine("Монитор: Подписка отменена\n");
    }

    public void SimulateFileChanges()
    {
        watcher.CreateFile("document.txt");
        watcher.ChangeFile("document.txt");
        watcher.DeleteFile("document.txt");
    }
}

class Program
{
    static void Main()
    {
        FileMonitor monitor = new FileMonitor();

        Console.WriteLine("=== ОТСЛЕЖИВАНИЕ ИЗМЕНЕНИЙ ФАЙЛОВ ===\n");
        monitor.SimulateFileChanges();

        Console.WriteLine("\n--- Проверка лог-файла ---");
        if (File.Exists("file_log.txt"))
        {
            string log = File.ReadAllText("file_log.txt");
            Console.WriteLine(log);
        }

        Console.WriteLine("\nОтписываем подписчиков:");
        monitor.Unsubscribe();

        Console.WriteLine("Изменения больше не отслеживаются:");
        monitor.SimulateFileChanges();
    }
}