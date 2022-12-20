using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Best_Champ_Predictor
{
    /// <summary>
    /// Dateipfade und Funktionen zum Laden, Speichern und Löschen von UserSettings
    /// </summary>
    public static class Config
    {
        //Data Management

        public static string BasePath = AppDomain.CurrentDomain.BaseDirectory;
        public static string ConfigFile = BasePath + "Config.txt";
        public static string Seperator = ";";

        //Data Management RawData

        public static string RawDataPath = BasePath + "RawData";
        public static string RawTopPath = RawDataPath + "\\Top";
        public static string RawJunglePath = RawDataPath + "\\Jungle";
        public static string RawMiddlePath = RawDataPath + "\\Middle";
        public static string RawBottomPath = RawDataPath + "\\Bottom";
        public static string RawSupportPath = RawDataPath + "\\Support";

        //Data Management Queries

        public static string QueriesPath = BasePath + "Queries";
        public static string QueriesTopPath = QueriesPath + "\\Top";
        public static string QueriesJunglePath = QueriesPath + "\\Jungle";
        public static string QueriesMiddlePath = QueriesPath + "\\Middle";
        public static string QueriesBottomPath = QueriesPath + "\\Bottom";
        public static string QueriesSupportPath = QueriesPath + "\\Support";

        //Import Configurations

        public static string CurrentPatch = "12.23";
        public static string CurrentRole = Constants.Support;
        public static string CurrentPatchQuery = "14";
        public static string CurrentTier = "platinum_plus";
        public static string CurrentRegion = "euw";

        //Analyzer Configurations

        public static int MinNumberOfGames = 1000;

        /// <summary>
        /// Überschreibt die die UserSettings mit den aktuell aktiven Settings
        /// </summary>
        public static void SaveUserSettings()
        {
            if (File.Exists(ConfigFile))
                File.Delete(ConfigFile);

            using (StreamWriter writer = new StreamWriter(ConfigFile))
            {
                writer.WriteLine(CurrentRole);
                writer.WriteLine(CurrentPatchQuery);
                writer.WriteLine(CurrentTier);
                writer.WriteLine(CurrentRegion);
                writer.WriteLine(MinNumberOfGames);
            }

            Memory.SetRoleFolderPath();

            Console.WriteLine("User setting saved in file");
            Console.WriteLine(ConfigFile);
        }

        /// <summary>
        /// Schreibt die default Settings in die Config-Variablen (müssen danach noch evtl. abgespeichert werden und im Memory die Ordner neu gesetzt werden)
        /// </summary>
        public static void LoadDefaultConfig()
        {
            CurrentRole = Constants.Support;
            CurrentPatchQuery = "14";
            CurrentTier = "platinum_plus";
            CurrentRegion = "euw";
            MinNumberOfGames = 1000;
        }

        /// <summary>
        /// Lädt die vorher gespeicherten UserSettings aus der ConfigFile
        /// TODO: die Defaults im switch werfen noch ne Exception, weil ich versuche in die Datei zu schreiben, während der reader noch am lesen ist
        /// </summary>
        public static void LoadUserSettings()
        {
            if (!File.Exists(ConfigFile))
            {
                Console.WriteLine("No user settings loaded. You can configure your preferences by pressing [C].");
                return;
            }
            else
            {
                using (StreamReader reader = new StreamReader(ConfigFile))
                {
                    string roleSetting = reader.ReadLine();
                    switch (roleSetting)
                    {
                        case Constants.Top:
                        case Constants.Jungle:
                        case Constants.Middle:
                        case Constants.Bottom:
                        case Constants.Support:
                            CurrentRole = roleSetting;
                            break;

                        default:
                            LoadDefaultConfig();
                            SaveUserSettings();
                            Console.WriteLine(Constants.SettingsCorrupted);
                            return;
                    }

                    string patchQuerySetting = reader.ReadLine();
                    switch (patchQuerySetting)
                    {
                        case "7":
                        case "14":
                        case "30":
                            CurrentPatchQuery = patchQuerySetting;
                            break;

                        default:
                            LoadDefaultConfig();
                            SaveUserSettings();
                            Console.WriteLine(Constants.SettingsCorrupted);
                            return;
                    }

                    string eloSetting = reader.ReadLine();
                    switch (eloSetting)
                    {
                        case "d2_plus":
                        case "diamond_plus":
                        case "platinum_plus":
                        case "gold_plus":
                        case "silver":
                        case "bronze":
                            CurrentTier = eloSetting;
                            break;

                        default:
                            LoadDefaultConfig();
                            SaveUserSettings();
                            Console.WriteLine(Constants.SettingsCorrupted);
                            return;
                    }

                    string regionSetting = reader.ReadLine();
                    switch (regionSetting)
                    {
                        case "all":
                        case "kr":
                        case "euw":
                        case "eune":
                        case "na":
                        case "oce":
                            CurrentRegion = regionSetting;
                            break;

                        default:
                            LoadDefaultConfig();
                            SaveUserSettings();
                            Console.WriteLine(Constants.SettingsCorrupted);
                            return;
                    }

                    if (!Int32.TryParse(reader.ReadLine(), out int minGamesSetting))
                    {
                        LoadDefaultConfig();
                        SaveUserSettings();
                        Console.WriteLine(Constants.SettingsCorrupted);
                        return;
                    }
                    else
                    {
                        if (minGamesSetting < 0)
                        {
                            LoadDefaultConfig();
                            SaveUserSettings();
                            Console.WriteLine(Constants.SettingsCorrupted);
                            return;
                        }
                        else
                        {
                            MinNumberOfGames = minGamesSetting;
                        }
                    }
                }
            }

            Console.WriteLine("User settings loaded.");
        }
    }
}
