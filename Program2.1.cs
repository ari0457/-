using System;

class Program
{
    static void Main()
    {
        int[] arr = new int[5];
        Random rand = new Random();
        for (int i = 0; i < 5; i++)
        {
            arr[i] = rand.Next(-100, 100);
            Console.Write(arr[i] + " ");
        }
        Console.WriteLine();
        int minIndex = 0;
        for (int i = 1; i < 5; i++)
        {
            if (arr[i] < arr[minIndex])
                minIndex = i;
        }
        Console.WriteLine($"Минимальный элемент: {arr[minIndex]}, порядковый номер: {minIndex + 1}");
    }
}