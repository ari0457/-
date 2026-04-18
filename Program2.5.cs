using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите количество строк: ");
        int rows = Convert.ToInt32(Console.ReadLine());
        int[][] jagged = new int[rows][];
        Random rand = new Random();
        for (int i = 0; i < rows; i++)
        {
            jagged[i] = new int[rand.Next(2, 6)];
            for (int j = 0; j < jagged[i].Length; j++)
            {
                jagged[i][j] = rand.Next(1, 10);
            }
        }
        Console.WriteLine("Исходный массив:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < jagged[i].Length; j++)
            {
                Console.Write(jagged[i][j] + " ");
            }
            Console.WriteLine();
        }
        double[] averages = new double[rows];
        for (int i = 0; i < rows; i++)
        {
            double sum = 0;
            for (int j = 0; j < jagged[i].Length; j++)
            {
                sum += jagged[i][j];
            }
            averages[i] = sum / jagged[i].Length;
        }
        Array.Resize(ref jagged, rows + 1);
        jagged[rows] = new int[rows];
        for (int i = 0; i < rows; i++)
        {
            jagged[rows][i] = (int)averages[i];
        }
        Console.WriteLine("Массив после добавления строки:");
        for (int i = 0; i < rows + 1; i++)
        {
            for (int j = 0; j < jagged[i].Length; j++)
            {
                Console.Write(jagged[i][j] + " ");
            }
            Console.WriteLine();
        }
    }
}