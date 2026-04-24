using System;

public delegate void OrderProcessor(int orderId);

class OrderService
{
    public void ProcessOrder(int id, OrderProcessor processor)
    {
        Console.Write($"Заказ {id}: ");
        processor(id);
    }
}

class OrderHandler
{
    public void ApproveOrder(int id)
    {
        Console.WriteLine($"одобрен");
    }

    public void CancelOrder(int id)
    {
        Console.WriteLine($"отменен");
    }
}

class Program
{
    static void Main()
    {
        OrderService service = new OrderService();
        OrderHandler handler = new OrderHandler();

        service.ProcessOrder(1001, handler.ApproveOrder);
        service.ProcessOrder(1002, handler.CancelOrder);
        service.ProcessOrder(1003, handler.ApproveOrder);

        Console.WriteLine("\nОбработка нескольких заказов:");
        int[] orders = { 2001, 2002, 2003 };
        foreach (int id in orders)
        {
            OrderProcessor proc = id % 2 == 0 ? handler.ApproveOrder : handler.CancelOrder;
            service.ProcessOrder(id, proc);
        }
    }
}