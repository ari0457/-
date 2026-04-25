using System;

class SpeedLimitExceededException : Exception
{
    public SpeedLimitExceededException() { }
    public SpeedLimitExceededException(string message) : base(message) { }
    public SpeedLimitExceededException(string message, Exception inner) : base(message, inner) { }
}

class Car
{
    public void SetSpeed(int speed)
    {
        if (speed > 120)
        {
            throw new SpeedLimitExceededException($"Скорость {speed} км/ч превышает лимит 120 км/ч");
        }
        Console.WriteLine($"Скорость установлена: {speed} км/ч");
    }
}

class Program
{
    static void Main()
    {
        Car car = new Car();
        try
        {
            Console.Write("Введите скорость: ");
            int speed = Convert.ToInt32(Console.ReadLine());
            car.SetSpeed(speed);
        }
        catch (SpeedLimitExceededException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}