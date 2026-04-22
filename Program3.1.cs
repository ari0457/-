using System;

class Program
{
    static void Main()
    {
        A obj = new A(9, 2);
        Console.WriteLine($"a={obj.a}, b={obj.b}");
        Console.WriteLine($"Среднее арифметическое: {obj.Srednee()}");
        Console.WriteLine($"b^3 + sqrt(a) = {obj.Vyrazhenie()}");
    }
}