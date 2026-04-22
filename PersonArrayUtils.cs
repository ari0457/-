static class PersonArrayUtils
{
    private static Random rand = new Random();
    private static string[] names = { "Анна", "Иван", "Мария", "Петр", "Ольга", "Дмитрий", "Елена", "Сергей" };

    public static Person[] GenerateRandomPersons(int size)
    {
        Person[] persons = new Person[size];
        for (int i = 0; i < size; i++)
        {
            persons[i] = new Person();
            persons[i].Name = names[rand.Next(names.Length)];
            persons[i].Age = rand.Next(18, 70);
        }
        return persons;
    }
}
