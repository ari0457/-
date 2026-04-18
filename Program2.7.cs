using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        string input = Console.ReadLine();
        string upper = input.ToUpper();
        string lower = input.ToLower();
        Console.WriteLine($"Верхний регистр: {upper}");
        Console.WriteLine($"Нижний регистр: {lower}");
    }
}