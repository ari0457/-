using System;

interface IUser
{
    string GetPermissions();
}

class AdminUser : IUser
{
    public string GetPermissions()
    {
        return "Полный доступ: чтение, запись, удаление, управление пользователями";
    }
}

class ModeratorUser : IUser
{
    public string GetPermissions()
    {
        return "Модерация: удаление контента, блокировка пользователей";
    }
}

class RegularUser : IUser
{
    public string GetPermissions()
    {
        return "Базовый доступ: чтение, комментирование";
    }
}

abstract class UserFactory
{
    public abstract IUser CreateUser();
}

class AdminFactory : UserFactory
{
    public override IUser CreateUser()
    {
        return new AdminUser();
    }
}

class ModeratorFactory : UserFactory
{
    public override IUser CreateUser()
    {
        return new ModeratorUser();
    }
}

class RegularFactory : UserFactory
{
    public override IUser CreateUser()
    {
        return new RegularUser();
    }
}

class Program
{
    static void Main()
    {
        UserFactory[] factories = new UserFactory[]
        {
            new AdminFactory(),
            new ModeratorFactory(),
            new RegularFactory()
        };

        foreach (UserFactory factory in factories)
        {
            IUser user = factory.CreateUser();
            Console.WriteLine($"{user.GetType().Name}: {user.GetPermissions()}");
        }
    }
}