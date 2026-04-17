using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите x: ");
        double x = Convert.ToDouble(Console.ReadLine());
        double y;
        if (x > 1)
        {
            y = Math.Log(2 * x) + Math.Sqrt(1 + x * x);
        }
        else
        {
            y = 2 * Math.Cos(x) + 3 * x * x;
        }
        Console.WriteLine($"y = {y:F6}");
    }
}