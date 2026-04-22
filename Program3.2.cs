using System;

class Program
{
    static void Main()
    {
        Person[] people = PersonArrayUtils.GenerateRandomPersons(5);
        for (int i = 0; i < people.Length; i++)
        {
            Console.WriteLine($"Имя: {people[i].Name}, Возраст: {people[i].Age}");
        }
    }
}