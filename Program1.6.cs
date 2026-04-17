using System;

class Program
{
    static void Main()
    {
        Console.Write("Введите пол (м - мужчина, ж - женщина): ");
        string gender = Console.ReadLine();
        switch (gender)
        {
            case "м":
                Console.WriteLine("Мужские имена: Александр, Дмитрий, Иван, Михаил, Сергей");
                break;
            case "ж":
                Console.WriteLine("Женские имена: Анна, Мария, Екатерина, Ольга, Наталья");
                break;
            default:
                Console.WriteLine("Некорректный ввод");
                break;
        }
    }
}