using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите количество дней: ");
        int days = Convert.ToInt32(Console.ReadLine());
        int weeks = days / 7;
        Console.WriteLine($"Полных недель: {weeks}");
    }
}