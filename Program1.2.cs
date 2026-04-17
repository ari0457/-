using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите целое число: ");
        int number = Convert.ToInt32(Console.ReadLine());
        int absNumber = Math.Abs(number);
        bool isOddThreeDigit = (absNumber >= 100 && absNumber <= 999 && absNumber % 2 != 0);
        Console.WriteLine($"Число является нечетным трехзначным: {isOddThreeDigit}");
    }
}