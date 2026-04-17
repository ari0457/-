using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите A (-5 <= A <= 5): ");
        double A = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите N (1 <= N <= 10): ");
        int N = Convert.ToInt32(Console.ReadLine());

        double sum = 1;
        double power = 1;
        for (int i = 1; i <= N; i++)
        {
            power *= A;
            sum += power;
        }
        Console.WriteLine($"Сумма = {sum:F4}");
    }
}