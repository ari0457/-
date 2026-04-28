using System;
using System.Collections.Generic;

interface IDevice
{
    void Update(string mode);
}

class Light : IDevice
{
    private string name;

    public Light(string name)
    {
        this.name = name;
    }

    public void Update(string mode)
    {
        if (mode == "Ночь")
        {
            Console.WriteLine($"{name}: свет выключен");
        }
        else if (mode == "День")
        {
            Console.WriteLine($"{name}: свет включен");
        }
        else if (mode == "Вечер")
        {
            Console.WriteLine($"{name}: свет приглушен");
        }
    }
}

class Thermostat : IDevice
{
    private string name;

    public Thermostat(string name)
    {
        this.name = name;
    }

    public void Update(string mode)
    {
        if (mode == "Ночь")
        {
            Console.WriteLine($"{name}: температура 18°C");
        }
        else if (mode == "День")
        {
            Console.WriteLine($"{name}: температура 22°C");
        }
        else if (mode == "Вечер")
        {
            Console.WriteLine($"{name}: температура 20°C");
        }
    }
}

class Alarm : IDevice
{
    private string name;

    public Alarm(string name)
    {
        this.name = name;
    }

    public void Update(string mode)
    {
        if (mode == "Ночь")
        {
            Console.WriteLine($"{name}: охрана включена");
        }
        else if (mode == "День")
        {
            Console.WriteLine($"{name}: охрана выключена");
        }
        else if (mode == "Вечер")
        {
            Console.WriteLine($"{name}: охрана в режиме ожидания");
        }
    }
}

class SmartHomeHub
{
    private List<IDevice> devices = new List<IDevice>();
    private string currentMode;

    public void Subscribe(IDevice device)
    {
        devices.Add(device);
    }

    public void Unsubscribe(IDevice device)
    {
        devices.Remove(device);
    }

    public void SetMode(string mode)
    {
        currentMode = mode;
        Console.WriteLine($"\nРежим изменён на: {mode}");
        NotifyDevices();
    }

    private void NotifyDevices()
    {
        foreach (IDevice device in devices)
        {
            device.Update(currentMode);
        }
    }
}

class Program
{
    static void Main()
    {
        SmartHomeHub hub = new SmartHomeHub();

        Light livingLight = new Light("Гостиная");
        Light kitchenLight = new Light("Кухня");
        Thermostat thermostat = new Thermostat("Термостат");
        Alarm alarm = new Alarm("Сигнализация");

        hub.Subscribe(livingLight);
        hub.Subscribe(kitchenLight);
        hub.Subscribe(thermostat);
        hub.Subscribe(alarm);

        hub.SetMode("День");
        hub.SetMode("Вечер");
        hub.SetMode("Ночь");
    }
}