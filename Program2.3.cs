using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите N (N<10): ");
        int N = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите a: ");
        int a = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите b: ");
        int b = Convert.ToInt32(Console.ReadLine());
        Console.Write("Введите D: ");
        int D = Convert.ToInt32(Console.ReadLine());
        int[,] matrix = new int[N, N];
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                matrix[i, j] = rand.Next(a, b + 1);
                Console.Write(matrix[i, j] + "\t");
            }
            Console.WriteLine();
        }
        int countLessD = 0;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                if (matrix[i, j] < D)
                    countLessD++;
            }
        }
        Console.WriteLine($"Количество чисел, меньших {D}: {countLessD}");
        for (int j = 0; j < N; j++)
        {
            double sum = 0;
            for (int i = 0; i < N; i++)
            {
                sum += matrix[i, j];
            }
            Console.WriteLine($"Среднее арифметическое столбца {j + 1}: {sum / N:F2}");
        }
    }
}