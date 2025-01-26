using System;
using System.Collections.Generic;
using ProjetCombat;

public class Priest : Character, IManaUser
{
    public int CurrentMana { get; set; }
    public int MaxMana { get; set; }

    public Priest(string name) : base(name, 70, 0, 65, ArmorType.Fabric, 0.10, 0.0, 0.20, 70)
    {
        MaxMana = 100;
        CurrentMana = MaxMana;

        Abilities.Add(new Ability("Will-O-Wisp", 1, "Enemy", 15));
        Abilities.Add(new Ability("Fountain of Youth", 2, "Team", 30));
        Abilities.Add(new Ability("Black Hole", 3, "Enemy", 20));
    }

    public override void ChooseAction(List<Character> enemyTeam, List<Character> allyTeam)
    {
        Console.WriteLine($"What do you want to do with {Name} (Priest)?");
        Console.WriteLine("1. Physical attack (unavailable)");
        Console.WriteLine("2. Use a skill");
        Console.WriteLine("3. Skip the turn");

        int choice = int.Parse(Console.ReadLine() ?? "3");

        if (choice == 3)
        {
            Console.WriteLine($"{Name} skips their turn.");
            return;
        }

        if (choice == 2)
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
                    case "Will-O-Wisp":
                        ExecuteWillOWisp(enemyTeam);
                        break;
                    case "Fountain of Youth":
                        ExecuteFountainOfYouth(allyTeam);
                        break;
                    case "Black Hole":
                        ExecuteBlackHole(enemyTeam);
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid action.");
        }
    }

    private void ExecuteWillOWisp(List<Character> enemyTeam)
    {
        var target = SelectTarget(enemyTeam);
        if (target != null && CurrentMana >= 15)
        {
            double multiplier = (target is Paladin || target is Priest) ? 0.75 : 1.5;
            int damage = (int)(MagicalAttackPower * multiplier);
            Console.WriteLine($"{Name} uses Will-O-Wisp on {target.Name}, dealing {damage} damage.");
            target.TakeDamage(damage, DamageType.Magical);
            CurrentMana -= 15;
            Console.WriteLine("Stats after Will-O-Wisp:");
            Console.WriteLine("Caster:");
            DisplayStats();
            Console.WriteLine("Target:");
            target.DisplayStats();
        }
        else
        {
            Console.WriteLine("Not enough mana to use Will-O-Wisp.");
        }
    }

    private void ExecuteFountainOfYouth(List<Character> allyTeam)
    {
        if (CurrentMana >= 30)
        {
            int healing = (int)(MagicalAttackPower * 0.75);
            Console.WriteLine($"{Name} uses Fountain of Youth, healing each ally for {healing} points.");
            foreach (var ally in allyTeam)
            {
                if (ally.IsAlive)
                {
                    ally.Heal(healing);
                    Console.WriteLine("Stats after Fountain of Youth:");
                    Console.WriteLine("Caster:");
                    DisplayStats();
                    Console.WriteLine("Target:");
                    ally.DisplayStats();
                }
            }
            CurrentMana -= 30;
        }
        else
        {
            Console.WriteLine("Not enough mana to use Fountain of Youth.");
        }
    }

    private void ExecuteBlackHole(List<Character> enemyTeam)
    {
        var target = SelectTarget(enemyTeam);
        if (target != null && target is IManaUser manaUser && CurrentMana >= 20)
        {
            int manaBurned = Math.Max(40, manaUser.CurrentMana / 2);
            Console.WriteLine($"{Name} uses Black Hole on {target.Name}, reducing their mana by {manaBurned} points.");
            manaUser.CurrentMana -= manaBurned;
            CurrentMana -= 20;
            Console.WriteLine("Stats after Black Hole:");
            Console.WriteLine("Caster:");
            DisplayStats();
            Console.WriteLine("Target:");
            target.DisplayStats();
        }
        else
        {
            Console.WriteLine(target == null || !(target is IManaUser)
                ? "The target does not use mana. Black Hole has no effect."
                : "Not enough mana to use Black Hole.");
        }
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
