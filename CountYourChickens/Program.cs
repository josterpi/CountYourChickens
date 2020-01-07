using System;
using System.Collections.Generic;
using System.Linq;

namespace CountYourChickens
{
    public class Program
    {
        public const int GAME_RUNS = 10000;
        static void Main()
        {
            var board = new Board();
            var spinner = new Random();
            var spinnerValues = Enum.GetValues(typeof(Character));
            var results = new List<int>(GAME_RUNS);
            for (var i = 0; i < results.Capacity; i++)
            {
                board = new Board();
                while (true)
                {
                    var spin = (Character)spinnerValues.GetValue(spinner.Next(spinnerValues.Length));
                    if (spin == Character.Blank) board.Fox();
                    try
                    {
                        board.MoveTo(spin);
                    }
                    catch (GameOver)
                    {
                        break;
                    }
                }
                results.Add(board.ChicksInCoop);
            }

            Console.WriteLine("Wins: {0}", results.Where(x => x >= 40).Count());
            Console.WriteLine("Losses: {0}", results.Where(x => x < 40).Count());
            Console.WriteLine("Average: {0}", results.Average());

            Console.ReadKey();
        }
    }

    public enum Character
    {
        Blank = 0, // Blank on the board, Fox on the spinner
        Sheep,
        Pig,
        Tractor,
        Cow,
        Dog
    }
    public class Space
    {
        public Character Type { get; }
        public bool Bonus { get; }
        public bool LastSpace { get; }
        public Space(Character character, bool bonus = false, bool last = false)
        {
            Type = character;
            Bonus = bonus;
            LastSpace = last;
        }
    }
    public class Board
    {
        public List<Space> Spaces { get; }
        public int ChickenPosition { get; set; } = 0;
        public int ChicksInCoop { get; set; } = 0;
        public Board()
        {
            Spaces = new List<Space>
            {
                new Space(Character.Blank), // Starting Space
                new Space(Character.Blank),
                new Space(Character.Sheep),
                new Space(Character.Pig),
                new Space(Character.Tractor, bonus: true),
                new Space(Character.Cow),
                new Space(Character.Dog),
                new Space(Character.Pig),
                new Space(Character.Cow, bonus: true),
                new Space(Character.Dog),
                new Space(Character.Sheep),
                new Space(Character.Tractor),
                new Space(Character.Blank),
                new Space(Character.Cow),
                new Space(Character.Pig),
                new Space(Character.Blank),
                new Space(Character.Blank),
                new Space(Character.Blank),
                new Space(Character.Tractor),
                new Space(Character.Blank),
                new Space(Character.Tractor),
                new Space(Character.Dog),
                new Space(Character.Sheep, bonus: true),
                new Space(Character.Cow),
                new Space(Character.Dog),
                new Space(Character.Pig),
                new Space(Character.Tractor),
                new Space(Character.Blank),
                new Space(Character.Sheep),
                new Space(Character.Cow),
                new Space(Character.Blank),
                new Space(Character.Blank),
                new Space(Character.Tractor),
                new Space(Character.Pig),
                new Space(Character.Sheep),
                new Space(Character.Dog, bonus: true),
                new Space(Character.Blank),
                new Space(Character.Sheep),
                new Space(Character.Cow),
                new Space(Character.Pig, bonus: true),
                new Space(Character.Blank, last: true)
            };
        }
        public void Fox()
        {
            ChicksInCoop--;
        }
        public void MoveTo(Character character)
        {
            var moves = 1;
            for (var i = ChickenPosition + 1; i < Spaces.Count; i++)
            {
                if (character == Spaces[i].Type || Spaces[i].LastSpace)
                {
                    break;
                }
                moves++;
            }
            ChickenPosition += moves;
            ChicksInCoop += moves;
            if (Spaces[ChickenPosition].Bonus) ChicksInCoop++;
            if (ChickenPosition == Spaces.Count - 1) throw new GameOver();
        }
    }
    public class GameOver : Exception
    {
    }
}

