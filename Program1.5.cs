using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите целое число: ");
        int number = Convert.ToInt32(Console.ReadLine());
        bool endsWithSeven = Math.Abs(number) % 10 == 7;
        Console.WriteLine($"Число оканчивается цифрой 7: {endsWithSeven}");
    }
}