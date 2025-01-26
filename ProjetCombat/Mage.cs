using System;
using System.Collections.Generic;
using ProjetCombat;

public class Mage : Character, IManaUser
{
    public int CurrentMana { get; set; }
    public int MaxMana { get; set; }

    public Mage(string name) : base(name, 70, 0, 80, ArmorType.Fabric, 0.10, 0.0, 0.20, 60)
    {
        MaxMana = 100;
        CurrentMana = MaxMana;

        Abilities.Add(new Ability("Diamond Dust", 1, "Enemy", 10));
        Abilities.Add(new Ability("Freezing Coffin", 2, "Self", 20));
        Abilities.Add(new Ability("Vessel of Hatred", 3, "Enemy Team", 30));
    }

    public override void ChooseAction(List<Character> enemyTeam, List<Character> allyTeam)
    {
        Console.WriteLine($"What do you want to do with {Name} (Mage)?");
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
                    case "Diamond Dust":
                        ExecuteDiamondDust(enemyTeam);
                        break;
                    case "Freezing Coffin":
                        ExecuteFreezingCoffin();
                        break;
                    case "Vessel of Hatred":
                        ExecuteVesselOfHatred(enemyTeam);
                        break;
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid action.");
        }
    }

    private void ExecuteDiamondDust(List<Character> enemyTeam)
    {
        var target = SelectTarget(enemyTeam);
        if (target != null && CurrentMana >= 10)
        {
            Console.WriteLine($"{Name} uses Diamond Dust on {target.Name}!");
            target.TakeDamage(MagicalAttackPower, DamageType.Magical);
            CurrentMana -= 10;

            Console.WriteLine("Stats:");
            Console.WriteLine("Attacker:");
            DisplayStats();
            Console.WriteLine("Target:");
            target.DisplayStats();
        }
        else
        {
            Console.WriteLine("Not enough mana to use Diamond Dust.");
        }
    }

    private void ExecuteFreezingCoffin()
    {
        if (CurrentMana >= 20)
        {
            Console.WriteLine($"{Name} uses Freezing Coffin, increasing their resistances!");
            DodgeChance = Math.Min(DodgeChance + 0.2, 0.5);
            MagicResistanceChance = Math.Min(MagicResistanceChance + 0.2, 0.5);
            CurrentMana -= 20;

            Console.WriteLine("Stats after Freezing Coffin:");
            DisplayStats();
        }
        else
        {
            Console.WriteLine("Not enough mana to use Freezing Coffin.");
        }
    }

    private void ExecuteVesselOfHatred(List<Character> enemyTeam)
    {
        if (CurrentMana >= 30)
        {
            Console.WriteLine($"{Name} uses Vessel of Hatred, dealing damage to the entire enemy team!");
            foreach (var enemy in enemyTeam)
            {
                if (enemy.IsAlive)
                {
                    int damage = (int)(MagicalAttackPower * 0.75);
                    enemy.TakeDamage(damage, DamageType.Magical);

                    Console.WriteLine("Stats after Vessel of Hatred:");
                    Console.WriteLine("Attacker:");
                    DisplayStats();
                    Console.WriteLine("Target:");
                    enemy.DisplayStats();
                }
            }
            CurrentMana -= 30;
        }
        else
        {
            Console.WriteLine("Not enough mana to use Vessel of Hatred.");
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
