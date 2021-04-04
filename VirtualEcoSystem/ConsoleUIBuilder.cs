using Pastel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using Color = System.Drawing.Color;


namespace VirtualEcoSystem
{
    public static class ConsoleUIBuilder
    {
        public static void IntroScreen(string _playerName)
        {
            WriteLine($"Welcome {_playerName} to the {"Mojave Desert".Pastel("#ffc35c")}. Its hot here.");
            WriteLine("You will be in charge of this section of the Desert."
                + $"\nPlant {"seeds".Pastel("#40c2ff")}, harvest {"wildlife".Pastel("#40c2ff")}, craft {"items".Pastel("#40c2ff")} and {"buy".Pastel("#40c2ff")} or {"sell".Pastel("#40c2ff")} from the Local Market."
                + "\nAll in the name of science!"
                + "\nGood luck,\n\tMark"
                + "\n\np.s: Please don't destroy the environment like the last person.\n\n\n"
                );
        }

        public static void WaitForInput(string _msg = "Press any key to continue...")
        {
            WriteLine(_msg);
            ReadKey();
        }

        public static void DisplayPlayerTurns(Player _player)
        {
            int TotalPlayerTurns = _player.MaxTurns + _player.OverageTurns;
            Write("Turns: [");
            for (int i = 0; i < TotalPlayerTurns; i++)
            {
                if (i >= _player.MaxTurns)
                {
                    Write("#".Pastel("#7732a8"));
                }
                else if (i < _player.CurrentTurns)
                {
                    Write("#".Pastel("#fc352b"));
                }
                else
                {
                    Write("#".Pastel("#420907"));
                }

            }
            Write("]");
            WriteLine("");
        }

        public static string AskForPlayerName()
        {
            WriteLine("Here we are...");
            WriteLine("By the way, I didn't catch your "+"name".Pastel("#ff458f") + "? What was it?");
            string input = ReadLine().Trim().ToLower();

            return input;
        }

        public static int PlayerOptions(string[] playerOptions)
        {
            WriteLine("\nWhat do you want to do?");

            int count = 1;
            foreach(var option in playerOptions)
            {
                WriteLine($"{count++}) {option}");
            }

            string playerInput = ReadLine().Trim();
            int convertedInput =-1;
            try
            {
                convertedInput = Convert.ToInt32(playerInput);
            }
            catch(FormatException)
            {
                // TODO
            }

            if (convertedInput > 0 && convertedInput <= playerOptions.Length)
            {
                return convertedInput;
            }
            else
            {
                return -1;
            }
        }
    }
}
