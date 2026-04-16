using System;

class Program
{
    static void Main()
    {
        Console.Write("Цена тетради (руб.) — ");
        double priceNotebook = Convert.ToDouble(Console.ReadLine());

        Console.Write("Цена обложки (руб.) — ");
        double priceCover = Convert.ToDouble(Console.ReadLine());

        Console.Write("Количество комплектов (шт.) — ");
        int count = Convert.ToInt32(Console.ReadLine());

        double totalCost = (priceNotebook + priceCover) * count;

        Console.WriteLine($"Стоимость покупки: {totalCost:F2} руб.");
    }
}