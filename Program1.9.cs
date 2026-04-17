using System;

class Program
{
    static void Main()
    {
        double a = Math.PI / 4;
        double b = Math.PI / 2;
        int M = 15;
        double H = (b - a) / M;

        Console.WriteLine($"Табуляция функции ctg(x) на отрезке [{a:F4}, {b:F4}]");
        Console.WriteLine("x\t\tctg(x)");
        Console.WriteLine("-------------------------");

        for (int i = 0; i <= M; i++)
        {
            double x = a + i * H;
            double ctg = Math.Cos(x) / Math.Sin(x);
            Console.WriteLine($"{x:F6}\t{ctg:F6}");
        }
    }
}