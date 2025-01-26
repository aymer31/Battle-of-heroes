using System;
using ProjetCombat;

public class Ability
{
    public string Name { get; set; }
    public int MaxCooldown { get; set; } 
    public int CurrentCooldown { get; private set; } 
    public string TargetType { get; set; } 
    public int ManaCost { get; set; } 

    public Ability(string name, int maxCooldown, string targetType, int manaCost)
    {
        Name = name;
        MaxCooldown = maxCooldown;
        TargetType = targetType;
        ManaCost = manaCost;
        CurrentCooldown = 0; 
    }
    public bool IsAvailable => CurrentCooldown == 0;

    public void Use(Character user, Character target)
    {
        if (!IsAvailable)
        {
            Console.WriteLine($"{Name} is in cooldown for {CurrentCooldown} turn.");
            return;
        }

        if (user == null || target == null)
        {
            Console.WriteLine("target invalid.");
            return;
        }

        if (ManaCost > 0 && user is IManaUser manaUser && manaUser.CurrentMana < ManaCost)
        {
            Console.WriteLine($"{user.Name} not enough mana to use {Name}.");
            return;
        }

        if (user is IManaUser manaUserWithCost)
        {
            manaUserWithCost.CurrentMana -= ManaCost;
        }

        Console.WriteLine($"{user.Name} used {Name} on {target.Name}.");
        CurrentCooldown = MaxCooldown;
    }

    public void ReduceCooldown()
    {
        if (CurrentCooldown > 0)
        {
            CurrentCooldown--;
        }
    }

    public override string ToString()
    {
        return $"Ability: {Name}\n" +
               $"Cooldown: {MaxCooldown} turn\n" +
               $"Current Cooldown: {CurrentCooldown} turn\n" +
               $"Target Type: {TargetType}\n" +
               $"Mana Cost: {ManaCost}";
    }
}
