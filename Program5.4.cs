using System;

interface ISqlDatabase
{
    void Connect();
}

interface INoSqlDatabase
{
    void Connect();
}

class DatabaseConnector : ISqlDatabase, INoSqlDatabase
{
    void ISqlDatabase.Connect()
    {
        Console.WriteLine("Подключение к SQL базе данных");
    }

    void INoSqlDatabase.Connect()
    {
        Console.WriteLine("Подключение к NoSQL базе данных");
    }
}

class Program
{
    static void Main()
    {
        DatabaseConnector connector = new DatabaseConnector();

        ISqlDatabase sql = connector;
        sql.Connect();

        INoSqlDatabase nosql = connector;
        nosql.Connect();
    }
}