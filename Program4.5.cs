using System;

abstract class Device
{
    public abstract void TurnOn();
    public virtual void TurnOff()
    {
        Console.WriteLine("Device is turning off");
    }
}

class Smartphone : Device
{
    public override void TurnOn()
    {
        Console.WriteLine("Smartphone is turning on");
    }
    public override void TurnOff()
    {
        Console.WriteLine("Smartphone is turning off");
    }
}

class Laptop : Device
{
    public override void TurnOn()
    {
        Console.WriteLine("Laptop is turning on");
    }
    public override void TurnOff()
    {
        Console.WriteLine("Laptop is turning off");
    }
}

class Program
{
    static void Main()
    {
        Device phone = new Smartphone();
        Device laptop = new Laptop();
        phone.TurnOn();
        phone.TurnOff();
        laptop.TurnOn();
        laptop.TurnOff();
    }
}