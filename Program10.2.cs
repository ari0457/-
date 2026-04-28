using System;

interface IWeapon
{
    void Attack();
}

class Sword : IWeapon
{
    public void Attack()
    {
        Console.WriteLine("Удар мечом!");
    }
}

class Bow : IWeapon
{
    public void Attack()
    {
        Console.WriteLine("Выстрел из лука!");
    }
}

class MagicSpell : IWeapon
{
    public void Attack()
    {
        Console.WriteLine("Заклинание огня!");
    }
}

class Character
{
    private IWeapon weapon;

    public void SetWeapon(IWeapon weapon)
    {
        this.weapon = weapon;
    }

    public void Fight()
    {
        weapon.Attack();
    }
}

class Program
{
    static void Main()
    {
        Character hero = new Character();

        hero.SetWeapon(new Sword());
        hero.Fight();

        hero.SetWeapon(new Bow());
        hero.Fight();

        hero.SetWeapon(new MagicSpell());
        hero.Fight();
    }
}