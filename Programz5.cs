using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите трехзначное число: ");
        int number = Convert.ToInt32(Console.ReadLine());

        int lastDigit = number % 10;
        int firstTwoDigits = number / 10;

        int newNumber = lastDigit * 100 + firstTwoDigits;

        Console.WriteLine($"Полученное число: {newNumber}");
    }
}