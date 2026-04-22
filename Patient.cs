partial class Patient
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Diagnosis { get; set; }

    public Patient(string name, int age, string diagnosis)
    {
        Name = name;
        Age = age;
        Diagnosis = diagnosis;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Имя: {Name}, Возраст: {Age}, Диагноз: {Diagnosis}");
    }
}
