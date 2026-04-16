using System;

class Program
{
    static void Main()
    {
        Console.Write("a = ");
        double a = Convert.ToDouble(Console.ReadLine());

        Console.Write("b = ");
        double b = Convert.ToDouble(Console.ReadLine());

        double result = a / b;

        Console.WriteLine($"{a:F3}/{b:F3}={result:F3}");

        Console.WriteLine("Для продолжения нажмите любую клавишу . . .");
        Console.ReadKey();
    }
}