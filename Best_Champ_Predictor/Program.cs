using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Best_Champ_Predictor
{
    internal class Program
    {
        //Wenn es bereits Daten von Lolalytics gibt, so werden diese geladen und Preload auf true gesetzt
        //TODO: ist noch etwas buggy, wenn es bereits Daten gibt und man die neu importiert - die neu importierten Daten werden nicht in den Arbeitsspeicher geladen
        public static bool Preload = false;

        /// <summary>
        /// Initialisierung und Dauerschleife, welche das Menü neu lädt, bis man das Programm mit [ESC] verlässt
        /// </summary>
        /// <param name="args">keine Funktion</param>
        private static void Main(string[] args)
        {
            Initialize();
            while (true)
            {
                PrintMenu();
                WaitForInput();
            }
        }

        /// <summary>
        /// Wird beim Start ausgeführt, ändert Größe des Fensters, liest die UserSetting, und setzt die Pfade in Memory zum leichteren erschaffen und löschen von Ordnern/Dateien
        /// </summary>
        private static void Initialize()
        {
            SetWindow();
            Config.LoadUserSettings();

            Memory.DataList = new List<Importer.Testetsstsset>();
            Memory.SetRoleFolderPath();
            Memory.SetChampions();

            Loader loader = new Loader(out bool preload);
            Preload = preload;
        }

        /// <summary>
        /// Setzt das Fenstergröße abhängig von der Auflösung
        /// TODO: Anscheinend gibts noch Probleme, wenn der User mehrere Bildschirme hat
        /// </summary>
        private static void SetWindow()
        {
            int height = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            //Console.SetWindowSize(width / 30, height / 15);
        }

        /// <summary>
        /// Printet das Menu, welches die UserSettings und die Funktionen des Programms anzeigt
        /// </summary>
        private static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Your role  : " + Config.CurrentRole);
            Console.WriteLine("Your elo   : " + Config.CurrentTier);
            Console.WriteLine("Your server: " + Config.CurrentRegion);
            Console.WriteLine("Interval   : " + "last " + Config.CurrentPatchQuery + " days");
            Console.WriteLine("min. Games : " + Config.MinNumberOfGames);
            Console.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+");
            Console.WriteLine("[ESC] - Close programm");
            Console.WriteLine("[L]   - Set your lobby");
            Console.WriteLine("[C]   - Configure settings");
            Console.WriteLine("[I]   - Import Data from Lolalytics (ca. 20 seconds, depends on Lolalytics)");
            Console.WriteLine("[D]   - Delete All Data");

            if (Memory.Champions == null || Memory.ChampionID == null)
                Console.WriteLine(Constants.NoDataFound);
            else if (Memory.Champions.Count == 0 || Memory.ChampionID.Count == 0 || Memory.Champions.Count != Memory.ChampionID.Count)
                Console.WriteLine(Constants.CorruptedData);
        }

        /// <summary>
        /// Wartet, bis der User eine Taste gedrückt hat - abhängig von der Taste wird eine Programmfunktion ausgeführt
        /// </summary>
        public static void WaitForInput()
        {
            var input = Console.ReadKey(true);

            switch (input.Key)
            {
                //Wenn noch nichts im Arbeitsspeicher geladen ist, dann werden die importierten Daten geladen,
                //danach kann man die Lobby setten und die Winrates werden abhängig von der Lobby berechnet
                case ConsoleKey.L:
                    Console.WriteLine();
                    Console.WriteLine("Input: L");

                    //TODO: hier noch buggy bei einem neuen Import von Daten, es werden die alten Daten benutzt, bis man das Programm neu startet
                    if (!Preload)
                    {
                        Loader loader = new Loader(out bool success);
                        Console.WriteLine();

                        if (!success)
                        {
                            Console.WriteLine("There was an error loading the Data into memory. Please restart the program and try importing Data from Lolalytics again.");
                            Console.ReadLine();
                            return;
                        }
                    }

                    LobbySetter ls = new LobbySetter();
                    break;

                //Öffnet das ConfigMenu
                case ConsoleKey.C:
                    ConfigMenu configMenu = new ConfigMenu();
                    break;

                //Löscht alle Daten
                case ConsoleKey.D:
                    Console.WriteLine();
                    Console.WriteLine("Input: D");

                    Console.WriteLine("Are you sure that you want to delete all data? (y/n)");
                    input = Console.ReadKey(true);
                    if (input.Key == ConsoleKey.Y)
                    {
                        DeleteAllData();
                    }
                    else
                    {
                        Console.WriteLine(Constants.Cancelled);
                    }
                    break;

                //Importiert Daten von Lolalytics, filtert dann die wichtigen Werte heraus und speichert sie in einer Text-Datei ab
                case ConsoleKey.I:
                    Console.WriteLine();
                    Console.WriteLine("Input: I");

                    Console.WriteLine("This will override the files in the folder");
                    Console.WriteLine(Memory.RoleFolderPath);
                    Console.WriteLine("and in the folder");
                    Console.WriteLine(Memory.RoleFolderQueriesPath);
                    Console.WriteLine("Are you sure? (y/n)");
                    input = Console.ReadKey(true);

                    if (input.Key == ConsoleKey.Y)
                    {
                        ImporterAPI api = new ImporterAPI();
                        Importer importer = new Importer();
                    }
                    else
                    {
                        Console.WriteLine(Constants.Cancelled);
                    }
                    break;

                //Schließt das Programm
                case ConsoleKey.Escape:
                    System.Environment.Exit(0);
                    break;

                //Printet das Menu neu
                default:
                    break;
            }
        }

        /// <summary>
        /// Löscht alle importierten Daten und UserSettings
        /// </summary>
        public static void DeleteAllData()
        {
            if (!Directory.Exists(Config.QueriesPath) && !Directory.Exists(Config.RawDataPath) && !File.Exists(Config.ConfigFile))
            {
                Console.WriteLine("There was no data to delete");
                return;
            }

            if (Directory.Exists(Config.QueriesPath))
                Directory.Delete(Config.QueriesPath, true);
            if (Directory.Exists(Config.RawDataPath))
                Directory.Delete(Config.RawDataPath, true);
            if (File.Exists(Config.ConfigFile))
                File.Delete(Config.ConfigFile);

            Console.WriteLine("All Data deleted!");
        }
    }
}