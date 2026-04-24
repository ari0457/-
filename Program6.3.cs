using System;

public delegate void VolumeChangedHandler(int newVolume);

class VolumeControl
{
    private int volume = 50;
    public event VolumeChangedHandler VolumeChanged;

    public void Increase()
    {
        if (volume < 100)
        {
            volume += 10;
            VolumeChanged?.Invoke(volume);
        }
    }

    public void Decrease()
    {
        if (volume > 0)
        {
            volume -= 10;
            VolumeChanged?.Invoke(volume);
        }
    }
}

class Display
{
    public void ShowVolume(int volume)
    {
        Console.WriteLine($"Дисплей: Громкость установлена на {volume}%");
        Console.Write("Визуализация: ");
        Console.WriteLine(new string('|', volume / 5) + new string('.', 20 - volume / 5));
    }
}

class SpeakerSystem
{
    public void ChangeVolume(int volume)
    {
        Console.WriteLine($"Динамики: Громкость изменена на {volume}%");
        if (volume >= 80)
            Console.WriteLine("Динамики: Осторожно, высокая громкость!");
        else if (volume <= 20)
            Console.WriteLine("Динамики: Тихий режим");
        else
            Console.WriteLine("Динамики: Нормальный режим");
    }
}

class Program
{
    static void Main()
    {
        VolumeControl control = new VolumeControl();
        Display display = new Display();
        SpeakerSystem speakers = new SpeakerSystem();

        control.VolumeChanged += display.ShowVolume;
        control.VolumeChanged += speakers.ChangeVolume;

        Console.WriteLine("=== УПРАВЛЕНИЕ ГРОМКОСТЬЮ ===\n");

        Console.WriteLine("Увеличиваем громкость:");
        control.Increase();

        Console.WriteLine("\nЕще увеличиваем:");
        control.Increase();

        Console.WriteLine("\nУменьшаем громкость:");
        control.Decrease();

        Console.WriteLine("\nОтписываем дисплей и меняем громкость:");
        control.VolumeChanged -= display.ShowVolume;
        control.Increase();
    }
}