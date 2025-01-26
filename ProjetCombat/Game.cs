using System;
using System.Collections.Generic;
using System.Linq;
using ProjetCombat;

namespace ProjetCombat
{
    public class Game
    {
        public void Start()
        {
            Console.WriteLine("Welcome to the Battle of Heroes!");
            Console.WriteLine("Each player need to build a team of 3 Heroes.");
            Console.WriteLine("Class available: Warrior, Mage, Paladin, Rogue.");

            var player1Team = CreateTeam("Player 1");
            var player2Team = CreateTeam("Player 2");

            DisplayTeams(player1Team, player2Team);

            Console.WriteLine("\n--- It's time for the duel ! ---");

            while (true)
            {
                if (IsTeamDefeated(player1Team))
                {
                    Console.WriteLine("Player 2 take the win !");
                    break;
                }

                if (IsTeamDefeated(player2Team))
                {
                    Console.WriteLine("Player 1 take the win !");
                    break;
                }

                PlayRound(player1Team, player2Team);

                if (IsTeamDefeated(player1Team))
                {
                    Console.WriteLine("Player 2 take the win !");
                    break;
                }

                if (IsTeamDefeated(player2Team))
                {
                    Console.WriteLine("Player 1 take the win !");
                    break;
                }
            }

            Console.WriteLine("It's was a really good fight !");
        }

        private List<Character> CreateTeam(string playerName)
        {
            var team = new List<Character>();
            Console.WriteLine($"\n{playerName}, Form your teams :");
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"\nChoose a hero {i} (Warrior, Mage, Paladin, Rogue) :");
                string choice = Console.ReadLine()?.Trim().ToLower();

                Console.Write($"Give him a name {choice} : ");
                string name = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "warrior":
                        team.Add(new Warrior(name));
                        break;
                    case "mage":
                        team.Add(new Mage(name));
                        break;
                    case "paladin":
                        team.Add(new Paladin(name));
                        break;
                    case "rogue":
                        team.Add(new Rogue(name));
                        break;
                    default:
                        Console.WriteLine("Wrong answer . Try again.");
                        i--;
                        break;
                }
            }
            return team;
        }

        private void DisplayTeams(List<Character> player1Team, List<Character> player2Team)
        {
            Console.WriteLine("\n--- Player 1's team ---");
            foreach (var character in player1Team)
            {
                character.DisplayInfo();
            }

            Console.WriteLine("\n--- Player 2's team ---");
            foreach (var character in player2Team)
            {
                character.DisplayInfo();
            }
        }

        private bool IsTeamDefeated(List<Character> team)
        {
            return team.All(character => !character.IsAlive);
        }

        private void PlayRound(List<Character> team1, List<Character> team2)
        {
            var allCharacters = team1.Concat(team2)
                                     .Where(c => c.IsAlive)
                                     .OrderByDescending(c => c.Speed)
                                     .ThenBy(_ => Guid.NewGuid()) 
                                     .ToList();

            foreach (var character in allCharacters)
            {
                if (!character.IsAlive) continue;

                Console.WriteLine($"\nIt's the turn of {character.Name} ({character.GetType().Name}).");

                var enemyTeam = team1.Contains(character) ? team2 : team1;
                var allyTeam = team1.Contains(character) ? team1 : team2;

                character.ChooseAction(enemyTeam, allyTeam);


                Console.WriteLine("\n--- Stat of the hero ---");
                Console.WriteLine($"Ally team stats ({(team1.Contains(character) ? "Player 1" : "Player 2")}):");
                DisplayTeamStats(allyTeam);
                Console.WriteLine($"\nEnemy team stats ({(team1.Contains(character) ? "Player 2" : "Player 1")}):");
                DisplayTeamStats(enemyTeam);
                Console.WriteLine("-----------------------------------");

            }

            foreach (var character in allCharacters)
            {
                character.UpdateCooldowns();
            }
        }

        private void DisplayTeamStats(List<Character> team)
        {
            foreach (var character in team)
            {
                character.DisplayInfo();
            }
        }
    }
}
