using System;

class UISettings
{
    private static UISettings instance;
    private string theme;

    private UISettings()
    {
        theme = "светлая";
    }

    public static UISettings GetInstance()
    {
        if (instance == null)
        {
            instance = new UISettings();
        }
        return instance;
    }

    public void SetTheme(string theme)
    {
        this.theme = theme;
        Console.WriteLine($"Тема установлена: {theme}");
    }

    public string GetTheme()
    {
        return theme;
    }
}

class Program
{
    static void Main()
    {
        UISettings settings1 = UISettings.GetInstance();
        UISettings settings2 = UISettings.GetInstance();

        Console.WriteLine($"Текущая тема: {settings1.GetTheme()}");
        settings1.SetTheme("тёмная");
        Console.WriteLine($"Тема после изменения: {settings2.GetTheme()}");

        Console.WriteLine($"Один ли объект? {settings1 == settings2}");
    }
}