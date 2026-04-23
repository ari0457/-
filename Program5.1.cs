using System;

abstract class Transport
{
    public abstract void Move();
}

class Car : Transport
{
    public override void Move()
    {
        Console.WriteLine("Машина едет по дороге");
    }
}

class Bike : Transport
{
    public override void Move()
    {
        Console.WriteLine("Велосипед едет по велодорожке");
    }
}

class Airplane : Transport
{
    public override void Move()
    {
        Console.WriteLine("Самолет летит в небе");
    }
}

class Program
{
    static void Main()
    {
        Transport[] transports = new Transport[]
        {
            new Car(),
            new Bike(),
            new Airplane()
        };
        for (int i = 0; i < transports.Length; i++)
        {
            transports[i].Move();
        }
    }
}