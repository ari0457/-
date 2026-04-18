using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите строку для шифрования: ");
        string text = Console.ReadLine();
        Console.Write("Введите сдвиг: ");
        int shift = Convert.ToInt32(Console.ReadLine());
        string result = "";
        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                char offset = char.IsUpper(c) ? 'A' : 'a';
                result += (char)((c - offset + shift) % 26 + offset);
            }
            else
            {
                result += c;
            }
        }
        Console.WriteLine($"Зашифрованная строка: {result}");
    }
}