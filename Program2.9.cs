using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.Write("Введите исходную строку: ");
        string input = Console.ReadLine();
        StringBuilder sb = new StringBuilder(input);
        Console.Write("Введите слово для замены: ");
        string oldWord = Console.ReadLine();
        Console.Write("Введите новое слово: ");
        string newWord = Console.ReadLine();
        sb.Replace(oldWord, newWord);
        Console.WriteLine($"Результат: {sb}");
    }
}