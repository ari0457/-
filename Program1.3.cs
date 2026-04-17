using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите цену 1 кг конфет: ");
        double price = Convert.ToDouble(Console.ReadLine());
        for (double kg = 0.1; kg <= 1.0; kg += 0.1)
        {
            Console.WriteLine($"{kg:F1} кг = {price * kg:F4} руб.");
        }
    }
}