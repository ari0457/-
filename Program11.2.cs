using System;

interface ICar
{
    string GetFeatures();
    double GetPrice();
}

class BasicCar : ICar
{
    public string GetFeatures()
    {
        return "Базовая комплектация: двигатель, колёса, руль";
    }

    public double GetPrice()
    {
        return 15000;
    }
}

abstract class CarDecorator : ICar
{
    protected ICar car;

    public CarDecorator(ICar car)
    {
        this.car = car;
    }

    public virtual string GetFeatures()
    {
        return car.GetFeatures();
    }

    public virtual double GetPrice()
    {
        return car.GetPrice();
    }
}

class SunroofDecorator : CarDecorator
{
    public SunroofDecorator(ICar car) : base(car) { }

    public override string GetFeatures()
    {
        return car.GetFeatures() + ", люк на крыше";
    }

    public override double GetPrice()
    {
        return car.GetPrice() + 1000;
    }
}

class NavigationDecorator : CarDecorator
{
    public NavigationDecorator(ICar car) : base(car) { }

    public override string GetFeatures()
    {
        return car.GetFeatures() + ", навигационная система";
    }

    public override double GetPrice()
    {
        return car.GetPrice() + 1500;
    }
}

class LeatherSeatsDecorator : CarDecorator
{
    public LeatherSeatsDecorator(ICar car) : base(car) { }

    public override string GetFeatures()
    {
        return car.GetFeatures() + ", кожаные сиденья";
    }

    public override double GetPrice()
    {
        return car.GetPrice() + 2000;
    }
}

class Program
{
    static void Main()
    {
        ICar car = new BasicCar();
        Console.WriteLine($"{car.GetFeatures()}\nЦена: ${car.GetPrice()}\n");

        ICar carWithSunroof = new SunroofDecorator(new BasicCar());
        Console.WriteLine($"{carWithSunroof.GetFeatures()}\nЦена: ${carWithSunroof.GetPrice()}\n");

        ICar fullyLoaded = new LeatherSeatsDecorator(
                            new NavigationDecorator(
                            new SunroofDecorator(
                            new BasicCar())));
        Console.WriteLine($"{fullyLoaded.GetFeatures()}\nЦена: ${fullyLoaded.GetPrice()}");
    }
}