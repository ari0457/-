using System;

class App
{
    public string Name { get; set; }
    public App(string name)
    {
        Name = name;
    }
}

class Battery
{
    public int Capacity { get; set; }
    public Battery(int capacity)
    {
        Capacity = capacity;
    }
}

class User
{
    public string Name { get; set; }
    public User(string name)
    {
        Name = name;
    }
}

class Smartphone
{
    public App[] Apps { get; set; }
    public Battery Battery { get; private set; }
    public User Owner { get; set; }
    public string Model { get; set; }

    public Smartphone(string model, int batteryCapacity)
    {
        Model = model;
        Battery = new Battery(batteryCapacity);
        Apps = new App[0];
    }

    public void InstallApp(App app)
    {
        App[] newApps = new App[Apps.Length + 1];
        for (int i = 0; i < Apps.Length; i++)
        {
            newApps[i] = Apps[i];
        }
        newApps[Apps.Length] = app;
        Apps = newApps;
    }

    public void ShowInstalledApps()
    {
        Console.WriteLine($"Телефон: {Model}, владелец: {Owner?.Name ?? "нет"}");
        Console.WriteLine("Установленные приложения:");
        for (int i = 0; i < Apps.Length; i++)
        {
            Console.WriteLine($" - {Apps[i].Name}");
        }
    }
}

class Program
{
    static void Main()
    {
        Smartphone[] phones = new Smartphone[3];
        phones[0] = new Smartphone("iPhone 14", 3200);
        phones[1] = new Smartphone("Samsung S23", 3900);
        phones[2] = new Smartphone("Xiaomi 12", 4500);

        phones[0].Owner = new User("Анна");
        phones[1].Owner = new User("Иван");

        phones[0].InstallApp(new App("Telegram"));
        phones[0].InstallApp(new App("WhatsApp"));
        phones[1].InstallApp(new App("Instagram"));
        phones[1].InstallApp(new App("TikTok"));
        phones[2].InstallApp(new App("YouTube"));

        for (int i = 0; i < phones.Length; i++)
        {
            phones[i].ShowInstalledApps();
            Console.WriteLine();
        }
    }
}