using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Best_Champ_Predictor
{
    /// <summary>
    /// Ruft ein ConfigMenu auf, wo man die UserSettings ändern kann
    /// </summary>
    public class ConfigMenu
    {
        private bool _done = false;

        /// <summary>
        /// Dauerschleife, welche das Menü neu lädt, bis man zurück zum Hauptmenü mit [ESC] geht
        /// </summary>
        public ConfigMenu()
        {
            while (!_done)
            {
                Console.Clear();
                PrintConfigMenu();
                WaitForInput();
            }
        }

        /// <summary>
        /// Printet alle veränderbaren UserSettings (und den aktuellen Patch, welcher nur relevant ist, wenn ein neuer Champion released wird)
        /// </summary>
        private void PrintConfigMenu()
        {
            List<string> toPrintBindedKeys = new List<string>();
            toPrintBindedKeys.Add("[1]");
            toPrintBindedKeys.Add("[2]");
            toPrintBindedKeys.Add("[3]");
            toPrintBindedKeys.Add("[4]");
            toPrintBindedKeys.Add("[5]");
            toPrintBindedKeys.Add("[6]");
            toPrintBindedKeys.Add("[ESC]");

            List<string> toPrintDescription = new List<string>();
            toPrintDescription.Add(Constants.GetPatch);
            toPrintDescription.Add(Constants.CurrentRole);
            toPrintDescription.Add(Constants.EloFilter);
            toPrintDescription.Add(Constants.Region);
            toPrintDescription.Add(Constants.NumberOfGames);
            toPrintDescription.Add(Constants.ResetDefault);
            toPrintDescription.Add("Save and Quit");

            string patchQuery = Config.CurrentPatchQuery;
            switch (patchQuery)
            {
                case "7":
                    patchQuery = "last 7 days";
                    break;

                case "14":
                    patchQuery = "last 14 days";
                    break;

                case "30":
                    patchQuery = "last 30 days";
                    break;

                default:
                    break;
            }

            List<string> toPrintSetting = new List<string>();
            toPrintSetting.Add(patchQuery);
            toPrintSetting.Add(Config.CurrentRole);
            toPrintSetting.Add(Config.CurrentTier);
            toPrintSetting.Add(Config.CurrentRegion);
            toPrintSetting.Add(Config.MinNumberOfGames.ToString());
            toPrintSetting.Add("");
            toPrintSetting.Add("");

            int maxCharsKeyBind = 0;
            for (int i = 0; i < toPrintBindedKeys.Count; i++)
            {
                if (maxCharsKeyBind < toPrintBindedKeys[i].Length)
                    maxCharsKeyBind = toPrintBindedKeys[i].Length;
            }

            int maxCharsDescription = 0;
            for (int i = 0; i < toPrintDescription.Count; i++)
            {
                if (maxCharsDescription < toPrintDescription[i].Length)
                    maxCharsDescription = toPrintDescription[i].Length;
            }

            for (int i = 0; i < toPrintDescription.Count; i++)
            {
                string spacingFirst = " ";
                for (int j = 0; j < maxCharsKeyBind - toPrintBindedKeys[i].Length; j++)
                    spacingFirst += " ";

                string spacingSecond = " ";
                for (int j = 0; j < maxCharsDescription - toPrintDescription[i].Length; j++)
                    spacingSecond += " ";

                Console.WriteLine(toPrintBindedKeys[i] + spacingFirst + toPrintDescription[i] + ":" + spacingSecond + toPrintSetting[i]);
            }
        }

        /// <summary>
        /// Wartet, bis der User eine Taste gedrückt hat - abhängig von der Taste wird eine Programmfunktion ausgeführt
        /// </summary>
        private void WaitForInput()
        {
            var input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.D1:
                    PrintSetPatchQuery();
                    break;

                case ConsoleKey.D2:
                    PrintSetRole();
                    break;

                case ConsoleKey.D3:
                    PrintSetElo();
                    break;

                case ConsoleKey.D4:
                    PrintSetRegion();
                    break;

                case ConsoleKey.D5:
                    PrintSetMinNumberGames();
                    break;

                case ConsoleKey.D6:
                    Console.WriteLine("Are you sure that you want to reset your settings? (y/n)");
                    input = Console.ReadKey(true);
                    if (input.Key == ConsoleKey.Y)
                    {
                        Config.LoadDefaultConfig();
                        Config.SaveUserSettings();
                    }
                    break;

                case ConsoleKey.Escape:
                    _done = true;
                    Config.SaveUserSettings();
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// UserSetting, welches bestimmt welche Daten von Lolalytics importiert werden (bis jetzt nur die letzten 7, 14 und 30 Tage)
        /// TODO: erlauben Patchnummern zu setten (jedoch muss man vorher einbauen, dass das Programm automatisch erkennt was der aktuelle Patch ist - maybe RiotAPI ???)
        /// </summary>
        private void PrintSetPatchQuery()
        {
            bool done = false;

            while (!done)
            {
                Console.Clear();

                List<string> options = new List<string>();
                options.Add("last 7 days");
                options.Add("last 14 days");
                options.Add("last 30 days");

                Console.WriteLine(Constants.GetPatch + ":");
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine("[" + (i + 1) + "] " + options[i]);
                }
                Console.WriteLine("[ESC] Back");

                var input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        Config.CurrentPatchQuery = "7";
                        done = true;
                        break;

                    case ConsoleKey.D2:
                        Config.CurrentPatchQuery = "14";
                        done = true;
                        break;

                    case ConsoleKey.D3:
                        Config.CurrentPatchQuery = "30";
                        done = true;
                        break;

                    case ConsoleKey.Escape:
                        done = true;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Erlaubt es dem User seine Rolle zu setten (Top, Jungle, Middle, Bottom, Support) => Hat Auswirkungen, welche Rolle von LolalyticsAPI importiert wird
        /// </summary>
        private void PrintSetRole()
        {
            bool done = false;

            while (!done)
            {
                Console.Clear();

                List<string> options = new List<string>();
                options.Add(Constants.Top);
                options.Add(Constants.Jungle);
                options.Add(Constants.Middle);
                options.Add(Constants.Bottom);
                options.Add(Constants.Support);

                Console.WriteLine(Constants.GetPatch + ":");
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine("[" + (i + 1) + "] " + options[i]);
                }
                Console.WriteLine("[ESC] Back");

                var input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        Config.CurrentRole = Constants.Top;
                        done = true;
                        break;

                    case ConsoleKey.D2:
                        Config.CurrentRole = Constants.Jungle;
                        done = true;
                        break;

                    case ConsoleKey.D3:
                        Config.CurrentRole = Constants.Middle;
                        done = true;
                        break;

                    case ConsoleKey.D4:
                        Config.CurrentRole = Constants.Bottom;
                        done = true;
                        break;

                    case ConsoleKey.D5:
                        Config.CurrentRole = Constants.Support;
                        done = true;
                        break;

                    case ConsoleKey.Escape:
                        done = true;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Erlaubt es dem User seine Elo zu setten (D2+, Dia+, Plat+, Gold+, Silver, Bronze) => Hat Auswirkungen, welche bei von welchem Skillniveau von LolalyticsAPI importiert wird
        /// </summary>
        private void PrintSetElo()
        {
            bool done = false;

            while (!done)
            {
                Console.Clear();

                List<string> options = new List<string>();
                options.Add("Diamond 2+");
                options.Add("Diamond+");
                options.Add("Platinum+");
                options.Add("Gold+");
                options.Add("Silver");
                options.Add("Bronze");

                Console.WriteLine(Constants.GetPatch + ":");
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine("[" + (i + 1) + "] " + options[i]);
                }
                Console.WriteLine("[ESC] Back");

                var input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        Config.CurrentTier = "d2_plus";
                        done = true;
                        break;

                    case ConsoleKey.D2:
                        Config.CurrentTier = "diamond_plus";
                        done = true;
                        break;

                    case ConsoleKey.D3:
                        Config.CurrentTier = "platinum_plus";
                        done = true;
                        break;

                    case ConsoleKey.D4:
                        Config.CurrentTier = "gold_plus";
                        done = true;
                        break;

                    case ConsoleKey.D5:
                        Config.CurrentTier = "silver";
                        done = true;
                        break;

                    case ConsoleKey.D6:
                        Config.CurrentTier = "bronze";
                        done = true;
                        break;

                    case ConsoleKey.Escape:
                        done = true;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Erlaubt es dem User seine Region zu setten (Global, Korea, EUW, EUNE, NA, OCE) => Hat Auswirkungen, von welchem Server von LolalyticsAPI importiert wird
        /// </summary>
        private void PrintSetRegion()
        {
            bool done = false;

            while (!done)
            {
                Console.Clear();

                List<string> options = new List<string>();
                options.Add("Global");
                options.Add("Korea");
                options.Add("Europe West");
                options.Add("Europe North East");
                options.Add("North America");
                options.Add("Oceania");

                Console.WriteLine(Constants.GetPatch + ":");
                for (int i = 0; i < options.Count; i++)
                {
                    Console.WriteLine("[" + (i + 1) + "] " + options[i]);
                }
                Console.WriteLine("[ESC] Back");

                var input = Console.ReadKey(true);

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        Config.CurrentRegion = "all";
                        done = true;
                        break;

                    case ConsoleKey.D2:
                        Config.CurrentRegion = "kr";
                        done = true;
                        break;

                    case ConsoleKey.D3:
                        Config.CurrentRegion = "euw";
                        done = true;
                        break;

                    case ConsoleKey.D4:
                        Config.CurrentRegion = "eune";
                        done = true;
                        break;

                    case ConsoleKey.D5:
                        Config.CurrentRegion = "na";
                        done = true;
                        break;

                    case ConsoleKey.D6:
                        Config.CurrentRegion = "oce";
                        done = true;
                        break;

                    case ConsoleKey.Escape:
                        done = true;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Erlaubt es dem User die minimale Anzahl der TotalGames (insgesamte Anzahl von gespielten Spielen unabhängig vom Matchup auf der ausgewählten Rolle) zu setten
        /// => Hat Auswirkungen, welche Champions vom 'Analyzer.cs' verworfen werden (wenn die TotalGames Anzahl zu niedrig ist)
        /// </summary>
        private void PrintSetMinNumberGames()
        {
            bool done = false;

            while (!done)
            {
                Console.Clear();

                Console.WriteLine("Set the minimum amount of games a champion needs to have to be evaluated.");
                Console.WriteLine("Champions below that number of games will be ignored.");
                Console.WriteLine("Enter a positive number including 0 (Standard is 1000):");

                var input = Console.ReadLine();

                if (!Int32.TryParse(input, out int inputParsed))
                {
                    continue;
                }
                else
                {
                    if (inputParsed >= 0)
                    {
                        done = true;
                        Config.MinNumberOfGames = inputParsed;
                    }
                    else
                        continue;
                }
            }
        }
    }
}