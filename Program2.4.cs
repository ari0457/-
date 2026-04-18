using System;

class Program
{
    static void Main()
    {
        int rows = 23;
        int cols = 40;
        int[,] hall = new int[rows, cols];
        Random rand = new Random();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                hall[i, j] = rand.Next(0, 2);
            }
        }
        bool hasFreeSeats = false;
        for (int j = 0; j < cols; j++)
        {
            if (hall[0, j] == 0)
            {
                hasFreeSeats = true;
                break;
            }
        }
        Console.WriteLine($"Есть ли свободные места в первом ряду: {hasFreeSeats}");
    }
}