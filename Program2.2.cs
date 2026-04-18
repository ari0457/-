using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите n: ");
        int n = Convert.ToInt32(Console.ReadLine());
        int[] a = new int[n];
        Random rand = new Random();
        int positiveSum = 0, negativeCount = 0, zeroCount = 0;
        for (int i = 0; i < n; i++)
        {
            a[i] = rand.Next(-10, 10);
            Console.Write(a[i] + " ");
            if (a[i] > 0)
                positiveSum += a[i];
            else if (a[i] < 0)
                negativeCount++;
            else
                zeroCount++;
        }
        Console.WriteLine();
        Console.WriteLine($"Сумма положительных: {positiveSum}");
        Console.WriteLine($"Число отрицательных: {negativeCount}");
        Console.WriteLine($"Число нулевых: {zeroCount}");
    }
}