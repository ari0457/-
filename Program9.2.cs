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
        return $"{Name},{Age}";
    }

    public static Person FromString(string line)
    {
        string[] parts = line.Split(',');
        return new Person(parts[0], int.Parse(parts[1]));
    }
}

class PersonFileSplitter
{
    public void WritePeople(List<Person> people)
    {
        List<Person> adults = new List<Person>();
        List<Person> minors = new List<Person>();

        foreach (Person p in people)
        {
            if (p.Age >= 18)
                adults.Add(p);
            else
                minors.Add(p);
        }

        using (StreamWriter writer = new StreamWriter("adults.data"))
        {
            foreach (Person p in adults)
            {
                writer.WriteLine(p.ToString());
            }
        }

        using (StreamWriter writer = new StreamWriter("minors.data"))
        {
            foreach (Person p in minors)
            {
                writer.WriteLine(p.ToString());
            }
        }

        Console.WriteLine($"Записано взрослых: {adults.Count}");
        Console.WriteLine($"Записано несовершеннолетних: {minors.Count}");
    }
}

class Program
{
    static void Main()
    {
        List<Person> people = new List<Person>
        {
            new Person("Иван", 25),
            new Person("Мария", 30),
            new Person("Петр", 15),
            new Person("Анна", 10),
            new Person("Дмитрий", 45),
            new Person("Елена", 17)
        };

        PersonFileSplitter splitter = new PersonFileSplitter();
        splitter.WritePeople(people);
    }
}