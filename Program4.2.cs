using System;

class Program
{
    static void DigitCountSum(int K, out int C, out int S)
    {
        C = 0;
        S = 0;
        K = Math.Abs(K);
        while (K > 0)
        {
            S += K % 10;
            C++;
            K /= 10;
        }
    }

    static void Main()
    {
        int[] numbers = { 123, 4567, 890, 5, 10001 };
        for (int i = 0; i < numbers.Length; i++)
        {
            int C, S;
            DigitCountSum(numbers[i], out C, out S);
            Console.WriteLine($"Число {numbers[i]}: цифр = {C}, сумма = {S}");
        }
    }
}