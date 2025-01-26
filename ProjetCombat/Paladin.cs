using System;
using System.Collections.Generic;
using ProjetCombat;

public class Paladin : Character, IManaUser
{
    public int CurrentMana { get; set; }
    public int MaxMana { get; set; }

    public Paladin(string name) : base(name, 120, 60, 40, ArmorType.Plate, 0.10, 0.20, 0.30, 40)
    {
        MaxMana = 100;
        CurrentMana = MaxMana;

        Abilities.Add(new Ability("Excalibur", 1, "Enemy", 5));
        Abilities.Add(new Ability("Demacia", 1, "Enemy", 10));
        Abilities.Add(new Ability("Goddess Blessing", 1, "Ally", 25));
    }

    public override void ChooseAction(List<Character> enemyTeam, List<Character> allyTeam)
    {
        Console.WriteLine($"What do you want to do with {Name} (Paladin)?");
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
                Console.WriteLine($"{Name} attacks {target.Name} with a physical attack!");
                int damage = PhysicalAttackPower;
                target.TakeDamage(damage, DamageType.Physical);
                HealFromDamage(damage);

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
                Console.WriteLine($"{i + 1}. {Abilities[i].Name} (Cost: {Abilities[i].ManaCost} mana)");
            }

            int abilityChoice = int.Parse(Console.ReadLine() ?? "1") - 1;
            if (abilityChoice >= 0 && abilityChoice < Abilities.Count)
            {
                switch (Abilities[abilityChoice].Name)
                {
                    case "Excalibur":
                        ExecuteExcalibur(enemyTeam);
                        break;
                    case "Demacia":
                        ExecuteDemacia(enemyTeam);
                        break;
                    case "Goddess Blessing":
                        ExecuteGoddessBlessing(allyTeam);
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid action.");
        }
    }

    private void ExecuteExcalibur(List<Character> enemyTeam)
    {
        var target = SelectTarget(enemyTeam);
        if (target != null && CurrentMana >= 5)
        {
            Console.WriteLine($"{Name} uses Excalibur on {target.Name}!");
            target.TakeDamage(PhysicalAttackPower, DamageType.Physical);
            CurrentMana -= 5;
            HealFromDamage(PhysicalAttackPower);

            Console.WriteLine("Stats after Excalibur:");
            Console.WriteLine("Attacker:");
            DisplayStats();
            Console.WriteLine("Target:");
            target.DisplayStats();
        }
        else
        {
            Console.WriteLine("Not enough mana to use Excalibur.");
        }
    }

    private void ExecuteDemacia(List<Character> enemyTeam)
    {
        var target = SelectTarget(enemyTeam);
        if (target != null && CurrentMana >= 10)
        {
            Console.WriteLine($"{Name} uses Demacia on {target.Name}!");
            target.TakeDamage(MagicalAttackPower, DamageType.Magical);
            CurrentMana -= 10;
            HealFromDamage(MagicalAttackPower);

            Console.WriteLine("Stats after Demacia:");
            Console.WriteLine("Attacker:");
            DisplayStats();
            Console.WriteLine("Target:");
            target.DisplayStats();
        }
        else
        {
            Console.WriteLine("Not enough mana to use Demacia.");
        }
    }

    private void ExecuteGoddessBlessing(List<Character> allyTeam)
    {
        var target = SelectTarget(allyTeam);
        if (target != null && CurrentMana >= 25)
        {
            int healing = (int)(MagicalAttackPower * 1.25);
            Console.WriteLine($"{Name} uses Goddess Blessing on {target.Name}, healing them for {healing} points.");
            target.Heal(healing);
            CurrentMana -= 25;

            Console.WriteLine("Stats after Goddess Blessing:");
            Console.WriteLine("Caster:");
            DisplayStats();
            Console.WriteLine("Target:");
            target.DisplayStats();
        }
        else
        {
            Console.WriteLine("Not enough mana to use Goddess Blessing.");
        }
    }

    private void HealFromDamage(int damage)
    {
        int healing = (int)(damage * 0.5);
        Heal(healing);
        Console.WriteLine($"{Name} heals for {healing} points thanks to their special ability.");
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
