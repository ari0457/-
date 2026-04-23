using System;

class Program
{
    static void Increase(ref int value, int increment)
    {
        value += increment;
    }
    static void Increase(ref double value, double increment)
    {
        value += increment;
    }
    static void Main()
    {
        int number = 5;
        Increase(ref number, 3);
        Console.WriteLine(number);
        double realNumber = 2.5;
        Increase(ref realNumber, 1.5);
        Console.WriteLine(realNumber);
    }
}