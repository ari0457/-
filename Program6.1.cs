using System;

public delegate double DiscountCalculator(double price);

class StudentDiscount
{
    public double ApplyDiscount(double price)
    {
        return price * 0.9;
    }
}

class SeniorDiscount
{
    public double ApplyDiscount(double price)
    {
        return price * 0.85;
    }
}

class Program
{
    static void Main()
    {
        StudentDiscount student = new StudentDiscount();
        SeniorDiscount senior = new SeniorDiscount();

        DiscountCalculator calc = student.ApplyDiscount;

        double price = 1000;
        Console.WriteLine($"Обычная цена: {price} руб");
        Console.WriteLine($"Со скидкой студента: {calc(price)} руб");

        calc = senior.ApplyDiscount;
        Console.WriteLine($"Со скидкой пенсионера: {calc(price)} руб");

        calc += student.ApplyDiscount;
        Console.WriteLine($"\nВсе скидки:");
        foreach (DiscountCalculator d in calc.GetInvocationList())
        {
            Console.WriteLine($"  {d(price)} руб");
        }
    }
}