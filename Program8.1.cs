using System;
using System.Collections;

class ServiceRequest
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string RequestType { get; set; }

    public ServiceRequest(int id, string customerName, string requestType)
    {
        Id = id;
        CustomerName = customerName;
        RequestType = requestType;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Имя: {CustomerName}, Тип: {RequestType}";
    }
}

class ServiceRequestManager
{
    private Queue queue = new Queue();

    public void AddRequest(ServiceRequest request)
    {
        queue.Enqueue(request);
        Console.WriteLine($"Добавлена заявка: {request}");
    }

    public void ProcessRequest()
    {
        if (queue.Count > 0)
        {
            ServiceRequest request = (ServiceRequest)queue.Dequeue();
            Console.WriteLine($"Обработана заявка: {request}");
        }
        else
        {
            Console.WriteLine("Нет заявок в очереди");
        }
    }

    public void ShowAllRequests()
    {
        Console.WriteLine("\nВсе заявки в очереди:");
        foreach (ServiceRequest request in queue)
        {
            Console.WriteLine(request);
        }
    }

    public int GetCount()
    {
        return queue.Count;
    }
}

class Program
{
    static void Main()
    {
        ServiceRequestManager manager = new ServiceRequestManager();

        manager.AddRequest(new ServiceRequest(1, "Иван", "Ремонт"));
        manager.AddRequest(new ServiceRequest(2, "Мария", "Консультация"));
        manager.AddRequest(new ServiceRequest(3, "Петр", "Замена"));

        manager.ShowAllRequests();
        Console.WriteLine($"\nКоличество заявок: {manager.GetCount()}");

        manager.ProcessRequest();
        manager.ShowAllRequests();
    }
}