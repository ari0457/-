using System;
using System.Text.RegularExpressions;

class InvalidEmailException : Exception
{
    public InvalidEmailException() { }
    public InvalidEmailException(string message) : base(message) { }
    public InvalidEmailException(string message, Exception inner) : base(message, inner) { }
}

class EmailValidator
{
    public void ValidateEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, pattern))
        {
            throw new InvalidEmailException($"Некорректный email адрес: {email}");
        }
        Console.WriteLine($"Email валиден: {email}");
    }
}

class Program
{
    static void Main()
    {
        EmailValidator validator = new EmailValidator();
        try
        {
            Console.Write("Введите email: ");
            string email = Console.ReadLine();
            validator.ValidateEmail(email);
        }
        catch (InvalidEmailException ex)
        {
            Console.WriteLine($"Ошибка валидации: {ex.Message}");
        }
    }
}