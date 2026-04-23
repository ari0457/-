using System;

class Program
{
    static string ToBinary(int number)
    {
        return Convert.ToString(number, 2);
    }

    static void Main()
    {
        Console.Write("Введите число: ");
        int num = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine($"Двоичное представление: {ToBinary(num)}");
    }
}