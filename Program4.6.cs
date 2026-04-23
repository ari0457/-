using System;

class Program
{
    static void PrevDate(ref int D, ref int M, ref int Y)
    {
        int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        if ((Y % 4 == 0 && Y % 100 != 0) || (Y % 400 == 0))
            daysInMonth[1] = 29;
        D--;
        if (D == 0)
        {
            M--;
            if (M == 0)
            {
                M = 12;
                Y--;
            }
            D = daysInMonth[M - 1];
        }
    }

    static void Main()
    {
        int[,] dates = { { 1, 1, 2023 }, { 15, 3, 2024 }, { 1, 3, 2020 } };
        for (int i = 0; i < 3; i++)
        {
            int D = dates[i, 0];
            int M = dates[i, 1];
            int Y = dates[i, 2];
            PrevDate(ref D, ref M, ref Y);
            Console.WriteLine($"Предыдущая дата: {D:D2}.{M:D2}.{Y}");
        }
    }
}