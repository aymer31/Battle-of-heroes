using System;
using System.Collections.Generic;
using ProjetCombat;

public class Rogue : Character
{
    public Rogue(string name) : base(name, 80, 55, 0, ArmorType.Leather, 0.15, 0.25, 0.25, 100)
    {
        Abilities.Add(new Ability("Shadow Slash", 1, "Enemy", 0));
        Abilities.Add(new Ability("Shadow Form", 2, "Self", 0));
    }

    public override void ChooseAction(List<Character> enemyTeam, List<Character> allyTeam)
    {
        Console.WriteLine($"What do you want to do with {Name} (Rogue)?");
        Console.WriteLine("1. Physical attack");
        Console.WriteLine("2. Use a skill");
        Console.WriteLine("3. Skip the turn");

        int choice = int.Parse(Console.ReadLine() ?? "3");

        if (choice == 3)
        {
            Console.WriteLine($"{Name} skips their turn.");
            return;
        }

        if (choice == 1)
        {
            var target = SelectTarget(enemyTeam);
            if (target != null)
            {
                Console.WriteLine($"{Name} attacks {target.Name} with a quick physical attack!");
                target.TakeDamage(PhysicalAttackPower, DamageType.Physical);
                Console.WriteLine("Stats after the attack:");
                Console.WriteLine("Attacker:");
                DisplayStats();
                Console.WriteLine("Target:");
                target.DisplayStats();
            }
        }
        else if (choice == 2)
        {
            Console.WriteLine("Choose a skill:");
            for (int i = 0; i < Abilities.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Abilities[i].Name}");
            }

            int abilityChoice = int.Parse(Console.ReadLine() ?? "1") - 1;
            if (abilityChoice >= 0 && abilityChoice < Abilities.Count)
            {
                if (Abilities[abilityChoice].Name == "Shadow Slash")
                {
                    ExecuteShadowSlash(enemyTeam);
                }
                else if (Abilities[abilityChoice].Name == "Shadow Form")
                {
                    ExecuteShadowForm();
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid action.");
        }
    }

    private void ExecuteShadowSlash(List<Character> enemyTeam)
    {
        var target = SelectTarget(enemyTeam);
        if (target != null)
        {
            int damage = target.CurrentHealth <= target.MaxHealth / 2 ? (int)(PhysicalAttackPower * 1.5) : PhysicalAttackPower;
            Console.WriteLine($"{Name} uses Shadow Slash on {target.Name}, dealing {damage} damage.");
            target.TakeDamage(damage, DamageType.Physical);
            Console.WriteLine("Stats after Shadow Slash:");
            Console.WriteLine("Attacker:");
            DisplayStats();
            Console.WriteLine("Target:");
            target.DisplayStats();
        }
    }

    private void ExecuteShadowForm()
    {
        DodgeChance = Math.Min(DodgeChance + 0.2, 0.5);
        MagicResistanceChance = Math.Min(MagicResistanceChance + 0.2, 0.5);
        Console.WriteLine($"{Name} uses Shadow Form, increasing dodge and magic resistance to {DodgeChance * 100}% and {MagicResistanceChance * 100}% respectively.");
    }

    public void ExecuteBackstab(Character attacker)
    {
        if (attacker == null || !IsAlive) return;
        Console.WriteLine($"{Name} counterattacks with Backstab, dealing 15 damage to {attacker.Name}.");
        attacker.TakeDamage(15, DamageType.Physical);
        Console.WriteLine("Stats after Backstab:");
        Console.WriteLine("Attacker:");
        DisplayStats();
        Console.WriteLine("Target:");
        attacker.DisplayStats();
    }

    private Character SelectTarget(List<Character> team)
    {
        Console.WriteLine("Choose a target:");
        for (int i = 0; i < team.Count; i++)
        {
            if (team[i].IsAlive)
            {
                Console.WriteLine($"{i + 1}. {team[i].Name} ({team[i].CurrentHealth}/{team[i].MaxHealth})");
            }
        }

        int choice = int.Parse(Console.ReadLine() ?? "1") - 1;
        if (choice >= 0 && choice < team.Count && team[choice].IsAlive)
        {
            return team[choice];
        }

        Console.WriteLine("Invalid target.");
        return null;
    }
}
