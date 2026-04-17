using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите стоимость одной штуки товара (x): ");
        double x = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("\nЦикл for:");
        for (int count = 10; count <= 200; count += 10)
        {
            Console.WriteLine($"{count} шт. = {x * count:F2} руб.");
        }

        Console.WriteLine("\nЦикл while:");
        int i = 10;
        while (i <= 200)
        {
            Console.WriteLine($"{i} шт. = {x * i:F2} руб.");
            i += 10;
        }

        Console.WriteLine("\nЦикл do while:");
        int j = 10;
        do
        {
            Console.WriteLine($"{j} шт. = {x * j:F2} руб.");
            j += 10;
        } while (j <= 200);
    }
}