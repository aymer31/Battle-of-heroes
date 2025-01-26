using System;
using System.Collections.Generic;
using ProjetCombat;

public class Warrior : Character
{
    public Warrior(string name) : base(name, 100, 50, 0, ArmorType.Plate, 0.05, 0.25, 0.10, 50)
    {
        Abilities.Add(new Ability("Giga Strike", 1, "Enemy", 0));
        Abilities.Add(new Ability("Cry of War", 2, "Team", 0));
        Abilities.Add(new Ability("Judgment", 2, "Enemy Team", 0));
    }

    public override void ChooseAction(List<Character> enemyTeam, List<Character> allyTeam)
    {
        Console.WriteLine($"What do you want to do with {Name} (Warrior)?");
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
                target.TakeDamage(PhysicalAttackPower, DamageType.Physical);
                Console.WriteLine($"Stats after the attack:");
                target.DisplayStats();
                DisplayStats();
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
                switch (Abilities[abilityChoice].Name)
                {
                    case "Giga Strike":
                        ExecuteGigaStrike(enemyTeam);
                        break;
                    case "Cry of War":
                        ExecuteCryOfWar(allyTeam);
                        break;
                    case "Judgment":
                        ExecuteJudgment(enemyTeam);
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid action.");
        }
    }

    private void ExecuteGigaStrike(List<Character> enemyTeam)
    {
        var target = SelectTarget(enemyTeam);
        if (target != null)
        {
            Console.WriteLine($"{Name} uses Giga Strike on {target.Name}.");
            target.TakeDamage(PhysicalAttackPower, DamageType.Physical);
            Console.WriteLine($"Stats after Giga Strike:");
            target.DisplayStats();
            DisplayStats();
        }
    }

    private void ExecuteCryOfWar(List<Character> allyTeam)
    {
        Console.WriteLine($"{Name} uses Cry of War, increasing the physical attack of the entire allied team.");
        foreach (var ally in allyTeam)
        {
            if (ally is Warrior warrior)
            {
                warrior.PhysicalAttackPower += 25;
                Console.WriteLine($"{ally.Name}'s physical attack increases to {warrior.PhysicalAttackPower}.");
            }
        }
    }

    private void ExecuteJudgment(List<Character> enemyTeam)
    {
        Console.WriteLine($"{Name} uses Judgment, attacking the entire enemy team!");
        foreach (var enemy in enemyTeam)
        {
            if (enemy.IsAlive)
            {
                int damage = (int)(PhysicalAttackPower * 0.33);
                enemy.TakeDamage(damage, DamageType.Physical);
                Console.WriteLine($"Stats after Judgment on {enemy.Name}:");
                enemy.DisplayStats();
            }
        }
        DisplayStats();
    }
}
