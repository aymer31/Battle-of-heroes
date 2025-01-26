using System;
using System.Collections.Generic;

namespace ProjetCombat
{
    public abstract class Character
    {
        public string Name { get; private set; }
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }
        public int PhysicalAttackPower { get; protected set; }
        public int MagicalAttackPower { get; private set; }
        public ArmorType Armor { get; private set; }
        public double DodgeChance { get; protected set; } 
        public double ParryChance { get; private set; }
        public double MagicResistanceChance { get; protected set; } 
        public int Speed { get; private set; }
        public List<Ability> Abilities { get; private set; }

        public Character(string name, int maxHealth, int physicalAttackPower, int magicalAttackPower,
                         ArmorType armor, double dodgeChance, double parryChance,
                         double magicResistanceChance, int speed)
        {
            Name = name;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            PhysicalAttackPower = physicalAttackPower;
            MagicalAttackPower = magicalAttackPower;
            Armor = armor;
            DodgeChance = dodgeChance;
            ParryChance = parryChance;
            MagicResistanceChance = magicResistanceChance;
            Speed = speed;
            Abilities = new List<Ability>();
        }

        public bool IsAlive => CurrentHealth > 0;

        public abstract void ChooseAction(List<Character> enemyTeam, List<Character> allyTeam);

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}, Health: {CurrentHealth}/{MaxHealth}, Mana: {(this is IManaUser manaUser ? $"{manaUser.CurrentMana}/{manaUser.MaxMana}" : "N/A")}");
        }

        public virtual void UpdateCooldowns()
        {
            foreach (var ability in Abilities)
            {
                ability.ReduceCooldown();
            }
        }

        public void TakeDamage(int damage, DamageType damageType)
        {
            Random random = new Random();


            if (damageType == DamageType.Physical && random.NextDouble() < DodgeChance)
            {
                Console.WriteLine($"{Name} dodge the attack !");
                return;
            }

            if (damageType == DamageType.Physical && random.NextDouble() < ParryChance)
            {
                Console.WriteLine($"{Name} pary the attack !");
                damage = (int)(damage * 0.5);
            }

            if (damageType == DamageType.Magical && random.NextDouble() < MagicResistanceChance)
            {
                Console.WriteLine($"{Name} resist to the magical attack !");
                return;
            }

            double reduction = damageType switch
            {
                DamageType.Physical => Armor switch
                {
                    ArmorType.Fabric => 0.0,
                    ArmorType.Leather => 0.15,
                    ArmorType.Mesh => 0.30,
                    ArmorType.Plate => 0.45,
                    _ => 0.0
                },
                DamageType.Magical => Armor switch
                {
                    ArmorType.Fabric => 0.30,
                    ArmorType.Leather => 0.20,
                    ArmorType.Mesh => 0.10,
                    ArmorType.Plate => 0.0,
                    _ => 0.0
                },
                _ => 0.0
            };

            damage = (int)(damage * (1 - reduction));

            CurrentHealth -= damage;
            CurrentHealth = Math.Max(CurrentHealth, 0);

            Console.WriteLine($"{Name} took {damage} damage. Current health : {CurrentHealth}/{MaxHealth}");
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
            CurrentHealth = Math.Min(CurrentHealth, MaxHealth);
            Console.WriteLine($"{Name} restore {amount} health. Current health : {CurrentHealth}/{MaxHealth}");
        }

        protected Character SelectTarget(List<Character> team)
        {
            Console.WriteLine("Choose a target :");
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

            Console.WriteLine("Target invalid.");
            return null;
        }
                public void DisplayStats()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Health: {CurrentHealth}/{MaxHealth}");
            Console.WriteLine($"Mana: {(this is IManaUser manaUser ? $"{manaUser.CurrentMana}/{manaUser.MaxMana}" : "N/A")}");
            Console.WriteLine($"Physical attack power: {PhysicalAttackPower}");
            Console.WriteLine($"Magical attack power: {MagicalAttackPower}");
            Console.WriteLine($"Armor: {Armor}");
            Console.WriteLine($"dodge: {DodgeChance * 100}%");
            Console.WriteLine($"Pary: {ParryChance * 100}%");
            Console.WriteLine($"Magical Resistance: {MagicResistanceChance * 100}%");
        }
    }
}
