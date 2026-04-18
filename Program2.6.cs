using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        string s = Console.ReadLine();
        Dictionary<char, int> charCount = new Dictionary<char, int>();
        foreach (char c in s)
        {
            if (charCount.ContainsKey(c))
                charCount[c]++;
            else
                charCount[c] = 1;
        }
        int oddCount = 0;
        foreach (int count in charCount.Values)
        {
            if (count % 2 != 0)
                oddCount++;
        }
        bool canBePalindrome = oddCount <= 1;
        Console.WriteLine($"Можно ли перестановкой символов получить палиндром: {canBePalindrome}");
    }
}