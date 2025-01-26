using System;
using System.Collections.Generic;
using ProjetCombat;
public class Attaque
{
    public string Launcher { get; private set; }
    public string Target { get; private set; }
    public int Damage { get; private set; }
    public DamageType Type { get; private set; }

    public Attaque(string launcher, string target, int damage, DamageType type)
    {
        Launcher = launcher;
        Target = target;
        Damage = damage;
        Type = type;
    }

    public void ExecuteAttack(Character target)
    {
        Console.WriteLine($"{Launcher} attack {target.Name} with a attck {Type}!\n");

        target.TakeDamage(Damage, Type);

        Console.WriteLine($"{Launcher} inflict {Damage} damage at {target.Name}.\n");
    }
}
