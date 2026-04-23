using System;

class Program
{
    static int Calculate(int a, int b)
    {
        return a + b;
    }
    static double Calculate(double a, double b)
    {
        return a + b;
    }
    static void Main()
    {
        Console.WriteLine(Calculate(3, 2));
        Console.WriteLine(Calculate(3.5, 2.5));
    }
}