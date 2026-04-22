using System;

class Program
{
    static void Main()
    {
        Hospital hospital = new Hospital();
        hospital.Patients = new Patient[]
        {
            new Patient("Иван", 45, "Грипп"),
            new Patient("Мария", 30, "Ангина"),
            new Patient("Петр", 65, "Грипп"),
            new Patient("Анна", 25, "Простуда")
        };

        Patient oldest = hospital.GetOldestPatient();
        Console.WriteLine("Самый старший пациент:");
        oldest.PrintInfo();

        Console.WriteLine("\nПациенты с диагнозом 'Грипп':");
        Patient[] fluPatients = hospital.GetPatientsByDiagnosis("Грипп");
        for (int i = 0; i < fluPatients.Length; i++)
        {
            fluPatients[i].PrintInfo();
        }
    }
}