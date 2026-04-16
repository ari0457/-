using System;

class Program
{
    static void Main()
    {
        Console.Write("x = ");
        double x = Convert.ToDouble(Console.ReadLine());

        double y = x * Math.Exp(Math.Sin(x) + Math.Cos(x) * Math.Log(x));

        Console.WriteLine($"y = {y:F6}");
    }
}