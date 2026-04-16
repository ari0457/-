using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите двузначное число: ");
        int number = Convert.ToInt32(Console.ReadLine());

        int tens = number / 10;
        int units = number % 10;

        int newNumber = units * 10 + tens;

        Console.WriteLine($"Исходное число: {number}");
        Console.WriteLine($"Число после перестановки: {newNumber}");
    }
}