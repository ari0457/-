using System;

class Program
{
    static void Main()
    {
        Warehouse warehouse = new Warehouse();
        warehouse.Products = new Product[]
        {
            new Electronics("Ноутбук", 1200, 5),
            new Electronics("Телефон", 800, 10),
            new Clothing("Футболка", 25, 50),
            new Clothing("Джинсы", 60, 30)
        };

        Console.WriteLine($"Общая стоимость: {warehouse.GetTotalStockValue()}");
        Product expensive = warehouse.FindMostExpensiveProduct();
        Console.WriteLine($"Самый дорогой товар: {expensive.Name}, Цена: {expensive.Price}");
    }
}