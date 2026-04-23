using System;

interface IProgrammer
{
    void WriteCode();
}

interface IDesigner
{
    void DrawDesign();
}

abstract class ITSpecialist
{
    public string Name { get; set; }
    public ITSpecialist(string name)
    {
        Name = name;
    }
}

class BackendDeveloper : ITSpecialist, IProgrammer
{
    public BackendDeveloper(string name) : base(name) { }
    public void WriteCode()
    {
        Console.WriteLine($"{Name} пишет код на C#");
    }
}

class UXDesigner : ITSpecialist, IDesigner
{
    public UXDesigner(string name) : base(name) { }
    public void DrawDesign()
    {
        Console.WriteLine($"{Name} рисует дизайн в Figma");
    }
}

class Program
{
    static void Main()
    {
        ITSpecialist[] specialists = new ITSpecialist[]
        {
            new BackendDeveloper("Алексей"),
            new UXDesigner("Мария"),
            new BackendDeveloper("Дмитрий"),
            new UXDesigner("Елена"),
            new BackendDeveloper("Сергей")
        };

        Console.WriteLine("Программисты:");
        for (int i = 0; i < specialists.Length; i++)
        {
            if (specialists[i] is IProgrammer programmer)
            {
                programmer.WriteCode();
            }
        }
    }
}