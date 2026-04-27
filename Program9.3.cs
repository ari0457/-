using System;
using System.Collections.Generic;
using System.IO;

class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public override string ToString()
    {
        return $"{Name} ({Age} лет)";
    }
}

class PersonFileReader
{
    public List<Person> ReadPeople(string filePath)
    {
        List<Person> people = new List<Person>();
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2)
                {
                    people.Add(new Person(parts[0], int.Parse(parts[1])));
                }
            }
        }
        return people;
    }
}

class PersonProcessor
{
    public void GroupByAge(List<Person> people)
    {
        List<Person> young = new List<Person>();
        List<Person> middle = new List<Person>();
        List<Person> old = new List<Person>();

        foreach (Person p in people)
        {
            if (p.Age < 18)
                young.Add(p);
            else if (p.Age <= 40)
                middle.Add(p);
            else
                old.Add(p);
        }

        Console.WriteLine("\nМоложе 18 лет:");
        foreach (Person p in young)
        {
            Console.WriteLine($"  {p}");
        }

        Console.WriteLine("\nОт 18 до 40 лет:");
        foreach (Person p in middle)
        {
            Console.WriteLine($"  {p}");
        }

        Console.WriteLine("\nСтарше 40 лет:");
        foreach (Person p in old)
        {
            Console.WriteLine($"  {p}");
        }
    }
}

class Program
{
    static void Main()
    {
        PersonFileReader reader = new PersonFileReader();
        PersonProcessor processor = new PersonProcessor();

        List<Person> adults = reader.ReadPeople("adults.data");
        List<Person> minors = reader.ReadPeople("minors.data");

        List<Person> allPeople = new List<Person>();
        allPeople.AddRange(adults);
        allPeople.AddRange(minors);

        Console.WriteLine("Все люди:");
        foreach (Person p in allPeople)
        {
            Console.WriteLine($"  {p}");
        }

        processor.GroupByAge(allPeople);
    }
}