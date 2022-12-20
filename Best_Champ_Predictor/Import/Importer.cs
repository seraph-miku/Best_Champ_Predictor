using Best_Champ_Predictor.Import;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Best_Champ_Predictor
{
    /// <summary>
    /// Überschreibt die alten Daten, wandelt die angefragten JSONs aus dem 'ImporterAPI.cs' in Textdateien um, die nur die relevanten Daten beinhalten
    /// TODO: vielleicht in JSONs abspeichern, bis jetzt jedoch noch keine Probleme mit dem Format
    /// TODO: Listen lokal in einer Methode erzeugen und an andere Methoden übergeben, damit man nicht nach jedem Champion resetten muss
    /// </summary>
    public class Importer
    {
        //public string OwnRole = Config.CurrentRole;

        //public string OwnChamp = "unknown";
        //public int GameCount = 0;
        //public double AvgWR = 0;

        //public List<string> EnemyTop = new List<string>();
        //public List<double> EnemyTopWR = new List<double>();
        //public List<int> EnemyTopGames = new List<int>();

        //public List<string> EnemyJungle = new List<string>();
        //public List<double> EnemyJungleWR = new List<double>();
        //public List<int> EnemyJungleGames = new List<int>();

        //public List<string> EnemyMid = new List<string>();
        //public List<double> EnemyMidWR = new List<double>();
        //public List<int> EnemyMidGames = new List<int>();

        //public List<string> EnemyBot = new List<string>();
        //public List<double> EnemyBotWR = new List<double>();
        //public List<int> EnemyBotGames = new List<int>();

        //public List<string> EnemySupp = new List<string>();
        //public List<double> EnemySuppWR = new List<double>();
        //public List<int> EnemySuppGames = new List<int>();

        //public List<string> OwnTop = new List<string>();
        //public List<double> OwnTopWR = new List<double>();
        //public List<int> OwnTopGames = new List<int>();

        //public List<string> OwnJungle = new List<string>();
        //public List<double> OwnJungleWR = new List<double>();
        //public List<int> OwnJungleGames = new List<int>();

        //public List<string> OwnMid = new List<string>();
        //public List<double> OwnMidWR = new List<double>();
        //public List<int> OwnMidGames = new List<int>();

        //public List<string> OwnBot = new List<string>();
        //public List<double> OwnBotWR = new List<double>();
        //public List<int> OwnBotGames = new List<int>();

        //public List<string> OwnSupp = new List<string>();
        //public List<double> OwnSuppWR = new List<double>();
        //public List<int> OwnSuppGames = new List<int>();

        /// <summary>
        /// Anzahl aller Einträge für einen Champion in der dazugehörigen Rolle
        /// </summary>
        public int TotalCount
        {
            get
            {
                //return EnemyTop.Count + EnemyJungle.Count + EnemyMid.Count + EnemyBot.Count + EnemySupp.Count + OwnTop.Count + OwnJungle.Count + OwnMid.Count + OwnBot.Count + OwnSupp.Count;
                return 0;
            }
        }

        public Importer()
        {
            if (!Directory.Exists(Config.RawDataPath))
            {
                Directory.CreateDirectory(Config.RawDataPath);
                Console.WriteLine("Folder '" + Config.RawDataPath + "' created.");
            }

            DeleteOldDataForRole();
            JSONToList();
        }

        public class Testetsstsset
        {
            public string OwnRole = Config.CurrentRole;
            public String OwncHamp { get; set; }
            public Int32 GameCount { get; set; }
            public Double AvgWr { get; set; }

            public Team Own { get; set; } = new Team();
            public Team Enemy { get; set; } = new Team();

            public class Team
            {
                public List<ChampionWinrates> TOP { get; set; }
                public List<ChampionWinrates> JGL { get; set; }
                public List<ChampionWinrates> MID { get; set; }
                public List<ChampionWinrates> BoT { get; set; }
                public List<ChampionWinrates> SUp { get; set; }
            }

            public class ChampionWinrates
            {
                public String Champion { get; set; }
                public Double WR { get; set; }
                public Int32 Games { get; set; }
            }
        }

        /// <summary>
        /// Wandelt die angefragten JSONs aus dem 'ImporterAPI.cs' in Textdateien um, die nur die relevanten Daten beinhalten und speichert die Ergebnisse ab, resettet am Ende die Klassenvariablen
        /// TODO: evtl. Untermethoden erzeugen, damit diese Methode kürzer zum lesen ist
        /// </summary>
        public void JSONToList()
        {
            DirectoryInfo d = new DirectoryInfo(Memory.RoleFolderQueriesPath);
            FileInfo[] Files = d.GetFiles("*.txt");
            foreach (FileInfo file in Files)
            {
                var testdata = new Testetsstsset();
                using (StreamReader r = new StreamReader(file.FullName))
                {
                    string json = r.ReadToEnd();
                    if (json == "NULL\r\n")
                        continue;
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(json);
                    JObject parsed = JObject.Parse(json);

                    //OwnChamp = file.Name.Split('_')[0];
                    testdata.OwncHamp = file.Name.Split('_')[0];

                    //GameCount = (int)parsed["header"]["n"];
                    testdata.GameCount = myDeserializedClass.header.n;

                    if (testdata.GameCount == 0)
                        continue;

                    //AvgWR = (double)parsed["header"]["wr"];
                    testdata.AvgWr = myDeserializedClass.header.wr;

                    string vsChampion = "";
                    int vsGames = 0;
                    int vsWins = 0;
                    double vsWinrate = 0d;
                    IList<JToken> results = null;

                    testdata.Enemy.TOP = myDeserializedClass.enemy_top.ToChampionWinrates();
                    testdata.Enemy.JGL = myDeserializedClass.enemy_jungle.ToChampionWinrates();
                    testdata.Enemy.MID = myDeserializedClass.enemy_middle.ToChampionWinrates();
                    testdata.Enemy.BoT = myDeserializedClass.enemy_bottom.ToChampionWinrates();
                    testdata.Enemy.SUp = myDeserializedClass.enemy_support.ToChampionWinrates();
                    testdata.Own.TOP = myDeserializedClass.team_top.ToChampionWinrates();
                    testdata.Own.JGL = myDeserializedClass.team_jungle.ToChampionWinrates();
                    testdata.Own.MID = myDeserializedClass.team_middle.ToChampionWinrates();
                    testdata.Own.BoT = myDeserializedClass.team_bottom.ToChampionWinrates();
                    testdata.Own.SUp = myDeserializedClass.team_support.ToChampionWinrates();

                    //results = parsed["enemy_top"].Children().ToList();
                    //foreach (JToken vsMatchup in results)
                    //{s
                    //    vsChampion = Memory.GetChampionByCID((int)vsMatchup[0]);
                    //    vsChampion = new String(vsChampion.Where(Char.IsLetter).ToArray()).ToLower();
                    //    vsChampion = vsChampion == "nunuwillump" ? "nunu" : vsChampion;
                    //    vsGames = (int)vsMatchup[1];
                    //    vsWins = (int)vsMatchup[2];
                    //    vsWinrate = Math.Round(((double)vsWins / (double)vsGames) * 100, 2);

                    //    EnemyTop.Add(vsChampion);
                    //    EnemyTopWR.Add(vsWinrate);
                    //    EnemyTopGames.Add(vsGames);
                    //}

                    //results = parsed["enemy_jungle"].Children().ToList();
                    //foreach (JToken vsMatchup in results)
                    //{
                    //    vsChampion = Memory.GetChampionByCID((int)vsMatchup[0]);
                    //    vsChampion = new String(vsChampion.Where(Char.IsLetter).ToArray()).ToLower();
                    //    vsChampion = vsChampion == "nunuwillump" ? "nunu" : vsChampion;
                    //    vsGames = (int)vsMatchup[1];
                    //    vsWins = (int)vsMatchup[2];
                    //    vsWinrate = Math.Round(((double)vsWins / (double)vsGames) * 100, 2);

                    //    EnemyJungle.Add(vsChampion);
                    //    EnemyJungleWR.Add(vsWinrate);
                    //    EnemyJungleGames.Add(vsGames);
                    //}

                    //results = parsed["enemy_middle"].Children().ToList();
                    //foreach (JToken vsMatchup in results)
                    //{
                    //    vsChampion = Memory.GetChampionByCID((int)vsMatchup[0]);
                    //    vsChampion = new String(vsChampion.Where(Char.IsLetter).ToArray()).ToLower();
                    //    vsChampion = vsChampion == "nunuwillump" ? "nunu" : vsChampion;
                    //    vsGames = (int)vsMatchup[1];
                    //    vsWins = (int)vsMatchup[2];
                    //    vsWinrate = Math.Round(((double)vsWins / (double)vsGames) * 100, 2);

                    //    EnemyMid.Add(vsChampion);
                    //    EnemyMidWR.Add(vsWinrate);
                    //    EnemyMidGames.Add(vsGames);
                    //}

                    //results = parsed["enemy_bottom"].Children().ToList();
                    //foreach (JToken vsMatchup in results)
                    //{
                    //    vsChampion = Memory.GetChampionByCID((int)vsMatchup[0]);
                    //    vsChampion = new String(vsChampion.Where(Char.IsLetter).ToArray()).ToLower();
                    //    vsChampion = vsChampion == "nunuwillump" ? "nunu" : vsChampion;
                    //    vsGames = (int)vsMatchup[1];
                    //    vsWins = (int)vsMatchup[2];
                    //    vsWinrate = Math.Round(((double)vsWins / (double)vsGames) * 100, 2);

                    //    EnemyBot.Add(vsChampion);
                    //    EnemyBotWR.Add(vsWinrate);
                    //    EnemyBotGames.Add(vsGames);
                    //}

                    //results = parsed["enemy_support"].Children().ToList();
                    //foreach (JToken vsMatchup in results)
                    //{
                    //    vsChampion = Memory.GetChampionByCID((int)vsMatchup[0]);
                    //    vsChampion = new String(vsChampion.Where(Char.IsLetter).ToArray()).ToLower();
                    //    vsChampion = vsChampion == "nunuwillump" ? "nunu" : vsChampion;
                    //    vsGames = (int)vsMatchup[1];
                    //    vsWins = (int)vsMatchup[2];
                    //    vsWinrate = Math.Round(((double)vsWins / (double)vsGames) * 100, 2);

                    //    EnemySupp.Add(vsChampion);
                    //    EnemySuppWR.Add(vsWinrate);
                    //    EnemySuppGames.Add(vsGames);
                    //}

                    //if (OwnRole != Constants.Top)
                    //{
                    //    results = parsed["team_top"].Children().ToList();
                    //    foreach (JToken vsMatchup in results)
                    //    {
                    //        vsChampion = Memory.GetChampionByCID((int)vsMatchup[0]);
                    //        vsChampion = new String(vsChampion.Where(Char.IsLetter).ToArray()).ToLower();
                    //        vsChampion = vsChampion == "nunuwillump" ? "nunu" : vsChampion;
                    //        vsGames = (int)vsMatchup[1];
                    //        vsWins = (int)vsMatchup[2];
                    //        vsWinrate = Math.Round(((double)vsWins / (double)vsGames) * 100, 2);

                    //        OwnTop.Add(vsChampion);
                    //        OwnTopWR.Add(vsWinrate);
                    //        OwnTopGames.Add(vsGames);
                    //    }
                    //}

                    //if (OwnRole != Constants.Jungle)
                    //{
                    //    results = parsed["team_jungle"].Children().ToList();
                    //    foreach (JToken vsMatchup in results)
                    //    {
                    //        vsChampion = Memory.GetChampionByCID((int)vsMatchup[0]);
                    //        vsChampion = new String(vsChampion.Where(Char.IsLetter).ToArray()).ToLower();
                    //        vsChampion = vsChampion == "nunuwillump" ? "nunu" : vsChampion;
                    //        vsGames = (int)vsMatchup[1];
                    //        vsWins = (int)vsMatchup[2];
                    //        vsWinrate = Math.Round(((double)vsWins / (double)vsGames) * 100, 2);

                    //        OwnJungle.Add(vsChampion);
                    //        OwnJungleWR.Add(vsWinrate);
                    //        OwnJungleGames.Add(vsGames);
                    //    }
                    //}

                    //if (OwnRole != Constants.Middle)
                    //{
                    //    results = parsed["team_middle"].Children().ToList();
                    //    foreach (JToken vsMatchup in results)
                    //    {
                    //        vsChampion = Memory.GetChampionByCID((int)vsMatchup[0]);
                    //        vsChampion = new String(vsChampion.Where(Char.IsLetter).ToArray()).ToLower();
                    //        vsChampion = vsChampion == "nunuwillump" ? "nunu" : vsChampion;
                    //        vsGames = (int)vsMatchup[1];
                    //        vsWins = (int)vsMatchup[2];
                    //        vsWinrate = Math.Round(((double)vsWins / (double)vsGames) * 100, 2);

                    //        OwnMid.Add(vsChampion);
                    //        OwnMidWR.Add(vsWinrate);
                    //        OwnMidGames.Add(vsGames);
                    //    }
                    //}

                    //if (OwnRole != Constants.Bottom)
                    //{
                    //    results = parsed["team_bottom"].Children().ToList();
                    //    foreach (JToken vsMatchup in results)
                    //    {
                    //        vsChampion = Memory.GetChampionByCID((int)vsMatchup[0]);
                    //        vsChampion = new String(vsChampion.Where(Char.IsLetter).ToArray()).ToLower();
                    //        vsChampion = vsChampion == "nunuwillump" ? "nunu" : vsChampion;
                    //        vsGames = (int)vsMatchup[1];
                    //        vsWins = (int)vsMatchup[2];
                    //        vsWinrate = Math.Round(((double)vsWins / (double)vsGames) * 100, 2);

                    //        OwnBot.Add(vsChampion);
                    //        OwnBotWR.Add(vsWinrate);
                    //        OwnBotGames.Add(vsGames);
                    //    }
                    //}

                    //if (OwnRole != Constants.Support)
                    //{
                    //    results = parsed["team_support"].Children().ToList();
                    //    foreach (JToken vsMatchup in results)
                    //    {
                    //        vsChampion = Memory.GetChampionByCID((int)vsMatchup[0]);
                    //        vsChampion = new String(vsChampion.Where(Char.IsLetter).ToArray()).ToLower();
                    //        vsChampion = vsChampion == "nunuwillump" ? "nunu" : vsChampion;
                    //        vsGames = (int)vsMatchup[1];
                    //        vsWins = (int)vsMatchup[2];
                    //        vsWinrate = Math.Round(((double)vsWins / (double)vsGames) * 100, 2);

                    //        OwnSupp.Add(vsChampion);
                    //        OwnSuppWR.Add(vsWinrate);
                    //        OwnSuppGames.Add(vsGames);
                    //    }
                    //}
                }

                SaveData(testdata);
                //Reset();
            }
        }

        /// <summary>
        /// Speichert die Daten zu jedem Champion in den Klassenlisten in 'exactPath' ab
        /// </summary>
        public void SaveData(Testetsstsset testetsstsset)
        {
            //if (OwnRole == "unknown" || OwnRole == "" || OwnChamp == "unknown" || OwnChamp == "")
            //    return;

            string saveDirectoryPath = Config.RawDataPath;
            string saveDirectorySubPath = Memory.RoleFolderPath;
            string exactPath = saveDirectorySubPath + "\\" + testetsstsset.OwncHamp + ".txt";
            string seperator = Config.Seperator;

            if (!Directory.Exists(saveDirectoryPath))
            {
                Directory.CreateDirectory(saveDirectoryPath);
            }

            if (!Directory.Exists(saveDirectorySubPath))
                Directory.CreateDirectory(saveDirectorySubPath);

            if (File.Exists(exactPath))
                File.Delete(exactPath);

            using (StreamWriter writer = new StreamWriter(exactPath))
            {
                writer.Write(JsonConvert.SerializeObject(testetsstsset));

                //writer.WriteLine(testetsstsset.AvgWr + seperator + testetsstsset.GameCount);
                //writer.WriteLine();

                //for (int i = 0; i < EnemyTop.Count; i++)
                //{
                //    writer.WriteLine("enemyTop" + seperator + EnemyTop[i] + seperator + EnemyTopWR[i] + seperator + EnemyTopGames[i]);
                //}

                //writer.WriteLine();

                //for (int i = 0; i < EnemyJungle.Count; i++)
                //{
                //    writer.WriteLine("enemyJungle" + seperator + EnemyJungle[i] + seperator + EnemyJungleWR[i] + seperator + EnemyJungleGames[i]);
                //}
                //writer.WriteLine();

                //for (int i = 0; i < EnemyMid.Count; i++)
                //{
                //    writer.WriteLine("enemyMiddle" + seperator + EnemyMid[i] + seperator + EnemyMidWR[i] + seperator + EnemyMidGames[i]);
                //}
                //writer.WriteLine();

                //for (int i = 0; i < EnemyBot.Count; i++)
                //{
                //    writer.WriteLine("enemyBottom" + seperator + EnemyBot[i] + seperator + EnemyBotWR[i] + seperator + EnemyBotGames[i]);
                //}
                //writer.WriteLine();

                //for (int i = 0; i < EnemySupp.Count; i++)
                //{
                //    writer.WriteLine("enemySupport" + seperator + EnemySupp[i] + seperator + EnemySuppWR[i] + seperator + EnemySuppGames[i]);
                //}
                //writer.WriteLine();

                //if (OwnRole != "top")
                //{
                //    for (int i = 0; i < OwnTop.Count; i++)
                //    {
                //        writer.WriteLine("ownTop" + seperator + OwnTop[i] + seperator + OwnTopWR[i] + seperator + OwnTopGames[i]);
                //    }
                //    writer.WriteLine();
                //}

                //if (OwnRole != "jungle")
                //{
                //    for (int i = 0; i < OwnJungle.Count; i++)
                //    {
                //        writer.WriteLine("ownJungle" + seperator + OwnJungle[i] + seperator + OwnJungleWR[i] + seperator + OwnJungleGames[i]);
                //    }
                //    writer.WriteLine();
                //}

                //if (OwnRole != "middle")
                //{
                //    for (int i = 0; i < OwnMid.Count; i++)
                //    {
                //        writer.WriteLine("ownMiddle" + seperator + OwnMid[i] + seperator + OwnMidWR[i] + seperator + OwnMidGames[i]);
                //    }
                //    writer.WriteLine();
                //}

                //if (OwnRole != "bottom")
                //{
                //    for (int i = 0; i < OwnBot.Count; i++)
                //    {
                //        writer.WriteLine("ownBottom" + seperator + OwnBot[i] + seperator + OwnBotWR[i] + seperator + OwnBotGames[i]);
                //    }
                //    writer.WriteLine();
                //}

                //if (OwnRole != "support")
                //{
                //    for (int i = 0; i < OwnSupp.Count; i++)
                //    {
                //        writer.WriteLine("ownSupport" + seperator + OwnSupp[i] + seperator + OwnSuppWR[i] + seperator + OwnSuppGames[i]);
                //    }
                //    writer.WriteLine();
                //}
            }

            Console.WriteLine("File '" + exactPath + "' created with " + TotalCount + " entries.");
        }

        /// <summary>
        /// Resettet die Klassenvariablen - muss nach jedem Champion einmal aufgerufen werden
        /// TODO: besseres System überlegen, damit man nicht andauernd resetten muss
        /// </summary>
        public void Reset()
        {
            //OwnChamp = "unknown";
            //GameCount = 0;
            //AvgWR = 0;

            //EnemyTop = new List<string>();
            //EnemyTopWR = new List<double>();
            //EnemyTopGames = new List<int>();

            //EnemyJungle = new List<string>();
            //EnemyJungleWR = new List<double>();
            //EnemyJungleGames = new List<int>();

            //EnemyMid = new List<string>();
            //EnemyMidWR = new List<double>();
            //EnemyMidGames = new List<int>();

            //EnemyBot = new List<string>();
            //EnemyBotWR = new List<double>();
            //EnemyBotGames = new List<int>();

            //EnemySupp = new List<string>();
            //EnemySuppWR = new List<double>();
            //EnemySuppGames = new List<int>();

            //OwnTop = new List<string>();
            //OwnTopWR = new List<double>();
            //OwnTopGames = new List<int>();

            //OwnJungle = new List<string>();
            //OwnJungleWR = new List<double>();
            //OwnJungleGames = new List<int>();

            //OwnMid = new List<string>();
            //OwnMidWR = new List<double>();
            //OwnMidGames = new List<int>();

            //OwnBot = new List<string>();
            //OwnBotWR = new List<double>();
            //OwnBotGames = new List<int>();

            //OwnSupp = new List<string>();
            //OwnSuppWR = new List<double>();
            //OwnSuppGames = new List<int>();
        }

        /// <summary>
        /// Löscht den alten Ordner 'Memory.RoleFolderPath'
        /// </summary>
        public void DeleteOldDataForRole()
        {
            if (Directory.Exists(Memory.RoleFolderPath))
            {
                DirectoryInfo d = new DirectoryInfo(Memory.RoleFolderPath);
                int entries = d.GetFiles("*.txt").Length;
                Directory.Delete(Memory.RoleFolderPath, true);

                Console.WriteLine("Folder '" + Memory.RoleFolderPath + "' with " + entries + " entries deleted!");
            }

            Directory.CreateDirectory(Memory.RoleFolderPath);
            Console.WriteLine("Folder '" + Memory.RoleFolderPath + "' created.");
        }
    }
}
