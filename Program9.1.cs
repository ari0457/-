using System;
using System.IO;
using System.Text;

class FileManager
{
    public void CreateFile(string path, string content)
    {
        File.WriteAllText(path, content);
        Console.WriteLine($"Файл создан: {path}");
    }

    public void DeleteFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Console.WriteLine($"Файл удален: {path}");
        }
        else
        {
            Console.WriteLine($"Ошибка: файл {path} не существует");
        }
    }

    public void CopyFile(string sourcePath, string destPath)
    {
        File.Copy(sourcePath, destPath, true);
        Console.WriteLine($"Файл скопирован из {sourcePath} в {destPath}");
    }

    public void MoveFile(string sourcePath, string destPath)
    {
        File.Move(sourcePath, destPath);
        Console.WriteLine($"Файл перемещен из {sourcePath} в {destPath}");
    }

    public void RenameFile(string oldPath, string newPath)
    {
        File.Move(oldPath, newPath);
        Console.WriteLine($"Файл переименован в {newPath}");
    }

    public void DeleteFilesByPattern(string directory, string pattern)
    {
        string[] files = Directory.GetFiles(directory, pattern);
        foreach (string file in files)
        {
            File.Delete(file);
            Console.WriteLine($"Удален файл: {file}");
        }
    }

    public void ListFiles(string directory)
    {
        string[] files = Directory.GetFiles(directory);
        Console.WriteLine("\nФайлы в директории:");
        foreach (string file in files)
        {
            Console.WriteLine(file);
        }
    }

    public void SetReadOnly(string path, bool isReadOnly)
    {
        FileInfo fileInfo = new FileInfo(path);
        fileInfo.IsReadOnly = isReadOnly;
        Console.WriteLine($"Файл {(isReadOnly ? "защищен от записи" : "доступен для записи")}: {path}");
    }

    public void CheckFilePermissions(string path)
    {
        FileInfo fileInfo = new FileInfo(path);
        Console.WriteLine($"\nПрава доступа для {path}:");
        Console.WriteLine($"  Чтение: {File.Exists(path)}");
        Console.WriteLine($"  Запись: {!fileInfo.IsReadOnly}");
    }
}

class FileInfoProvider
{
    public void GetFileInfo(string path)
    {
        if (File.Exists(path))
        {
            FileInfo info = new FileInfo(path);
            Console.WriteLine($"\nИнформация о файле {path}:");
            Console.WriteLine($"  Размер: {info.Length} байт");
            Console.WriteLine($"  Дата создания: {info.CreationTime}");
            Console.WriteLine($"  Дата изменения: {info.LastWriteTime}");
        }
        else
        {
            Console.WriteLine($"Файл {path} не существует");
        }
    }

    public long GetFileSize(string path)
    {
        FileInfo info = new FileInfo(path);
        return info.Length;
    }

    public bool CompareFilesBySize(string path1, string path2)
    {
        long size1 = GetFileSize(path1);
        long size2 = GetFileSize(path2);
        bool areEqual = size1 == size2;
        Console.WriteLine($"\nСравнение файлов:");
        Console.WriteLine($"  {path1}: {size1} байт");
        Console.WriteLine($"  {path2}: {size2} байт");
        Console.WriteLine($"  Файлы {(areEqual ? "одинакового" : "разного")} размера");
        return areEqual;
    }
}

class Program
{
    static void Main()
    {
        string dir = AppDomain.CurrentDomain.BaseDirectory;
        string fileName = "Dolidik.ar";
        string filePath = Path.Combine(dir, fileName);
        string copyPath = Path.Combine(dir, "Dolidik_copy.ar");
        string movedPath = Path.Combine(dir, "moved", fileName);
        string renamedPath = Path.Combine(dir, "Dolidik.io");
        string testFile = Path.Combine(dir, "test.ar");

        FileManager fm = new FileManager();
        FileInfoProvider fip = new FileInfoProvider();

        fm.CreateFile(filePath, "Привет, мир! Это тестовый файл.");

        string content = File.ReadAllText(filePath);
        Console.WriteLine($"\nСодержимое файла: {content}");

        fip.GetFileInfo(filePath);

        fm.CopyFile(filePath, copyPath);
        Console.WriteLine($"Копия существует: {File.Exists(copyPath)}");

        if (!Directory.Exists(Path.Combine(dir, "moved")))
            Directory.CreateDirectory(Path.Combine(dir, "moved"));
        fm.MoveFile(filePath, movedPath);

        fm.RenameFile(copyPath, renamedPath);

        fm.CreateFile(testFile, "Временный файл для удаления");
        fm.DeleteFile(testFile);

        fm.DeleteFilesByPattern(dir, "*.io");

        fm.ListFiles(dir);

        fm.SetReadOnly(renamedPath, true);
        try
        {
            File.WriteAllText(renamedPath, "Попытка записи в защищенный файл");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nОшибка записи: {ex.Message}");
        }
        fm.SetReadOnly(renamedPath, false);

        fm.CheckFilePermissions(renamedPath);

        fip.CompareFilesBySize(renamedPath, movedPath);
    }
}