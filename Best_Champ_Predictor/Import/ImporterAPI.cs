using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Best_Champ_Predictor
{
    internal class ImporterAPI
    {
        private string _winrateQueryPatch = Config.CurrentPatchQuery;
        private string _winrateQueryTier = Config.CurrentTier;
        private string _winrateQueryRegion = Config.CurrentRegion;
        private string _winrateQueryRole = Config.CurrentRole;

        private string _championRequest;
        private List<string> _winrateRequests;

        private List<string> _champions;
        private List<string> _championID;

        /// <summary>
        /// Löscht alle vorherigen Daten im Ordner "Memory.RoleFolderQueriesPath", welcher abhängig von der Rolle gesetzt wird (jeder Rolle hat einen eigenen Ordner)
        /// Holt per API-Abfragen von Lolalytics alle verfügbaren Daten in JSON-Format zu jedem Champion (nur auf der Rolle, welche in den UserSettings gespeichert ist)
        /// </summary>
        public ImporterAPI()
        {
            if (!Directory.Exists(Config.QueriesPath))
            {
                Directory.CreateDirectory(Config.QueriesPath);
                Console.WriteLine("Folder '" + Config.QueriesPath + "' created.");
            }

            DeleteOldDataForRole();
            CreateChampionsFile(out string championQueryResponseFile);

            if (_championRequest == null)
            {
                Console.WriteLine("Query 'Champion Request' failed. " + Constants.RestartProgram + " If the problem occurs again, try deleting the file '" + championQueryResponseFile + "'.");
                Console.ReadLine();
                return;
            }

            SetChampions();

            if (_champions == null || _championID == null || _champions.Count == 0 || _championID.Count == 0 || _champions.Count != _championID.Count)
            {
                Console.WriteLine("Error at parsing Champion Request Query. " + Constants.RestartProgram);
                Console.ReadLine();
            }

            CreateWinrateFiles();
            Memory.SetChampions();
        }

        /// <summary>
        /// lädt die Champions und die ChampionIDs in die Klassenlisten, um danach die Winrateanfragen an die LolalyticsAPI zu schicken
        /// TODO: Die champions.json von Lolalytics mit der JSON-Package von Newtonsoft lesen (wie im 'Importer.cs')
        /// </summary>
        private void SetChampions()
        {
            string[] championsSplit = _championRequest.Split(new string[] { "<h3>" }, StringSplitOptions.None);

            _champions = new List<string>(championsSplit.Length - 1);
            _championID = new List<string>(championsSplit.Length - 1);
            _championID.Add(championsSplit[0].TrimStart('"').Replace("\"", "").TrimEnd(':').Remove(0, 1));

            for (int i = 1; i <= championsSplit.Length - 2; i++)
            {
                string[] temp = championsSplit[i].Split(new string[] { "<\\/h3>" }, StringSplitOptions.None);
                _champions.Add(temp[0]);

                temp = temp[1].Split(new string[] { ",\"" }, StringSplitOptions.None);
                string cid = temp[1].TrimStart('"').Replace("\"", "").TrimEnd(':');
                _championID.Add(cid);
            }
            _champions.Add(championsSplit[championsSplit.Length - 1].Split(new string[] { "<\\/h3>" }, StringSplitOptions.None)[0]);
        }

        /// <summary>
        /// Sendet eine Anfrage an die LolalyticsAPI, um die champions.json zu bekommen
        /// </summary>
        private void ChampionQuery()
        {
            string championQueryAddress = "https://cdn.lolalytics.com/v/current/tooltip/eng/champion.json";

            using (var wb = new WebClient())
            {
                try
                {
                    _championRequest = wb.DownloadString(championQueryAddress);
                }
                catch
                {
                    Console.WriteLine("Query 'Champion Request' at '" + championQueryAddress + "' failed. " + Constants.RestartProgram);
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Sendet 2 Anfragen zu einem Champion an die LolalyticsAPI und fügt die beiden Antworten zu einem String zusammen
        /// </summary>
        /// <param name="championIndex">die ChampionID (von Lolalytics in der champions.json erzeugt) zu dem zugehörigen Champion</param>
        private bool WinrateQuery(int championIndex)
        {
            string[] winrateQuery = WinrateQueryBuilder(championIndex);

            using (var wb = new WebClient())
            {
                try
                {
                    string winrateRequest = wb.DownloadString(winrateQuery[0]);

                    if (winrateRequest == "")                                                         //Manche Champions werden so selten auf einer Rolle gespielt, dass keine JSON-Datei von Lolalytics zurückkommt
                        return false;

                    winrateRequest = winrateRequest.Substring(0, winrateRequest.Length - 1) + ",";      //ersetzt die letzte geschweifte Klammer des angekommenen JSON durch ein Komma, um beide Anfragen in eine zu verbinden
                    string secondRequest = wb.DownloadString(winrateQuery[1]);
                    winrateRequest += secondRequest.Substring(1);
                    _winrateRequests.Add(winrateRequest);
                    return true;
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp);
                    Console.WriteLine("Query 'Winrate Request' failed at '" + winrateQuery[0] + "'. " + Constants.RestartProgram);
                    Console.ReadLine();
                    return false;
                }
            }
        }

        /// <summary>
        /// erzeugt die Anfrage-Adressen an die LolalyticsAPI zu jedem Champion auf der Ausgewählten Position
        /// </summary>
        /// <param name="championIndex">die ChampionID (von Lolalytics in der champions.json erzeugt) zu dem zugehörigen Champion</param>
        /// <returns>Array mit zwei Adressen, da Lolalytics die relevanten Daten zu einem Champion in 2 Links speichert</returns>
        private string[] WinrateQueryBuilder(int championIndex)
        {
            string[] winrateQuery = new string[2];
            winrateQuery[0] = "https://axe.lolalytics.com/mega/?ep=champion&p=d&v=1&patch=" + _winrateQueryPatch + "&cid=" + _championID[championIndex] + "&lane="
                               + _winrateQueryRole + "&tier=" + _winrateQueryTier + "&queue=420&region=" + _winrateQueryRegion;
            winrateQuery[1] = "https://axe.lolalytics.com/mega/?ep=champion2&p=d&v=1&patch=" + _winrateQueryPatch + "&cid=" + _championID[championIndex] + "&lane="
                               + _winrateQueryRole + "&tier=" + _winrateQueryTier + "&queue=420&region=" + _winrateQueryRegion;
            return winrateQuery;
        }

        /// <summary>
        /// Holt sich die champions.json von Lolalytics und speichert sie in dem Ordner "Config.QueriesPath" ab
        /// </summary>
        /// <param name="championQueryResponseFile">Name der Datei, wo die champions.json von Lolalytics gespeichert wird
        /// (der 'Config.CurrentPatch' sollte nur relevant sein, wenn ein neuer Champion released wird)</param>
        private void CreateChampionsFile(out string championQueryResponseFile)
        {
            championQueryResponseFile = Config.QueriesPath + "\\" + "Champions_" + Config.CurrentPatch + ".txt";
            ChampionQuery();

            using (StreamWriter writer = new StreamWriter(championQueryResponseFile))
            {
                writer.WriteLine(_championRequest);
                Console.WriteLine("File '" + championQueryResponseFile + "' created.");
            }
        }

        /// <summary>
        /// Macht zu jedem Champion auf der gesetzten Position eine Anfrage an die LolalyticsAPI und speichert das Ergebnis in der 'winrateQueryResponseFile' ab
        /// </summary>
        private void CreateWinrateFiles()
        {
            _winrateRequests = new List<string>(_champions.Count);

            for (int i = 0; i < _champions.Count; i++)
            {
                string winrateQueryResponseFile = Memory.RoleFolderQueriesPath + "\\" + _champions[i] + "_" + Config.CurrentPatch + ".txt";
                WinrateQuery(i);

                using (StreamWriter writer = new StreamWriter(winrateQueryResponseFile))
                {
                    writer.WriteLine(_winrateRequests[i]);
                    Console.WriteLine("File '" + winrateQueryResponseFile + "' created.");
                }
            }
        }

        /// <summary>
        /// Löscht alle vorherigen Daten im Ordner "Memory.RoleFolderQueriesPath", welcher abhängig von der Rolle gesetzt wird (jeder Rolle hat einen eigenen Ordner)
        /// </summary>
        public void DeleteOldDataForRole()
        {
            if (Directory.Exists(Memory.RoleFolderQueriesPath))
            {
                DirectoryInfo d = new DirectoryInfo(Memory.RoleFolderQueriesPath);
                int entries = d.GetFiles("*.txt").Length;
                Directory.Delete(Memory.RoleFolderQueriesPath, true);

                Console.WriteLine("Folder '" + Memory.RoleFolderQueriesPath + "' with " + entries + " entries deleted!");
            }

            Directory.CreateDirectory(Memory.RoleFolderQueriesPath);
            Console.WriteLine("Folder '" + Memory.RoleFolderQueriesPath + "' created.");
        }
    }
}
