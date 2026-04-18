using System;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        Console.Write("Введите строку для проверки (IP-адрес): ");
        string ip = Console.ReadLine();
        string pattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
        bool isValid = Regex.IsMatch(ip, pattern);
        Console.WriteLine($"Является ли строка корректным IP-адресом: {isValid}");
    }
}