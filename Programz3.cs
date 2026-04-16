using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите угол a (в градусах): ");
        double aDegrees = Convert.ToDouble(Console.ReadLine());

        double a = aDegrees * Math.PI / 180;

        double cosA = Math.Cos(a);
        double sinA = Math.Sin(a);
        double z1 = (cosA + sinA) / (cosA - sinA);

        double tg2a = Math.Tan(2 * a);
        double sec2a = 1 / Math.Cos(2 * a);
        double z2 = tg2a + sec2a;

        Console.WriteLine($"\nРезультаты вычислений:");
        Console.WriteLine($"z1 = ({cosA:F4} + {sinA:F4}) / ({cosA:F4} - {sinA:F4}) = {z1:F6}");
        Console.WriteLine($"z2 = tg(2a) + sec(2a) = {tg2a:F6} + {sec2a:F6} = {z2:F6}");

        double difference = Math.Abs(z1 - z2);
        if (difference < 0.0001)
            Console.WriteLine($"\nРезультаты совпадают (разница: {difference:F8})");
        else
            Console.WriteLine($"\nРезультаты не совпадают (разница: {difference:F8})");
    }
}