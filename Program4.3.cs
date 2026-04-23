using System;

class Program
{
    static string ReverseString(string str)
    {
        if (str.Length <= 1)
            return str;
        return ReverseString(str.Substring(1)) + str[0];
    }

    static void Main()
    {
        Console.Write("Введите строку: ");
        string input = Console.ReadLine();
        Console.WriteLine($"Перевернутая строка: {ReverseString(input)}");
    }
}