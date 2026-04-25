using System;

class ApiException : Exception
{
    public ApiException() { }
    public ApiException(string message) : base(message) { }
    public ApiException(string message, Exception inner) : base(message, inner) { }
}

class ApiClient
{
    public void SendRequest(string url)
    {
        throw new HttpRequestException("Сервер недоступен");
    }
}

class RequestHandler
{
    public void HandleRequest(string url)
    {
        try
        {
            ApiClient client = new ApiClient();
            client.SendRequest(url);
        }
        catch (HttpRequestException ex)
        {
            throw new ApiException("Ошибка при отправке запроса к API", ex);
        }
    }
}

class Program
{
    static void Main()
    {
        RequestHandler handler = new RequestHandler();
        try
        {
            handler.HandleRequest("https://api.example.com/data");
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            Console.WriteLine($"Внутреннее исключение: {ex.InnerException?.GetType().Name}");
            Console.WriteLine($"Стек вызовов: {ex.StackTrace}");
        }
    }
}