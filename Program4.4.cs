using System;

static class DateTimeExtensions
{
    public static int GetAge(this DateTime birthDate)
    {
        DateTime today = DateTime.Today;
        int age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age))
            age--;
        return age;
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Введите дату рождения (гггг-мм-дд): ");
        DateTime birth = Convert.ToDateTime(Console.ReadLine());
        Console.WriteLine($"Возраст: {birth.GetAge()} лет");
    }
}