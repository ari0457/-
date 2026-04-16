using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите R1: ");
        double R1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введите R2: ");
        double R2 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Введите R3: ");
        double R3 = Convert.ToDouble(Console.ReadLine());

        double R = 1 / (1 / R1 + 1 / R2 + 1 / R3);

        Console.WriteLine($"Общее сопротивление: {R:F3} Ом");
    }
}