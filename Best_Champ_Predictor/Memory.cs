using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Best_Champ_Predictor.Importer;

namespace Best_Champ_Predictor
{
    /// <summary>
    /// Handlet die Ordnderpfade, die abhängig von der Rolle gesetzt werden
    /// Lädt die Champions mit ihrer zugehörigen ID (ID von Lolalytics vergeben) in den Arbeitsspeicher, um aus den Winrate-Queries (Importer-API) die relevanten Daten herauszufiltern
    /// Die DataList beinhaltet die Winrate und die Anzahl der Games von jedem Matchup von jedem Champion auf der gesetzten Rolle (UserSettings)
    /// </summary>
    public static class Memory
    {
        //beinhaltet die Winrate und die Anzahl der Games von jedem Matchup von jedem Champion auf der gesetzten Rolle (UserSettings)
        //public static List<DataTable> DataList;
        public static List<Testetsstsset> DataList;

        //relevante Ordner, die abhängig von der Rolle gesetzt werden

        public static string RoleFolderPath;
        public static string RoleFolderQueriesPath;

        //Alle Champions im Game und die dazugehörige ChampionID (ChampionID von Lolalytics)

        public static List<string> Champions;
        public static List<string> ChampionID;

        /// <summary>
        /// Hängt an die DataList eine neue leere DataTable an, welche alle Matchups von EINEM Champion auf EINER Position beinhalten wird (wird im 'AppendRowByID' befüllt)
        /// </summary>
        /// <param name="role">durch UserSettings gesetzte Rolle des Champions</param>
        /// <param name="champ">Name der Champions</param>
        /// <param name="winrate">Durchschnittliche Winrate des Champions auf der gesetzten Rolle</param>
        /// <param name="games">Anzahl der Games des Champions auf der gesetzten Rolle</param>
        /// <param name="tableID">wird benutzt, um die richtige DataTable im 'AppendRowByID' zu befüllen</param>
        public static void AppendToDataList(Testetsstsset item, out int tableID)
        {
            //DataTable append = new DataTable(role + " - " + champ + " - " + winrate + " - " + games);
            //append = SetColumns(append);
            tableID = DataList.Count;
            DataList.Add(item);
        }

        /// <summary>
        /// setzt die Standard-Columns für jede angehängte DataTable
        /// </summary>
        /// <param name="dt">Table, welche in die DataList integriert wird</param>
        /// <returns></returns>
        private static DataTable SetColumns(DataTable dt)
        {
            if (dt.Columns.Count != 0)
                return dt;

            dt.Columns.Add("Position", typeof(string));
            dt.Columns.Add("Champion", typeof(string));
            dt.Columns.Add("Winrate", typeof(double));
            dt.Columns.Add("Games", typeof(int));

            return dt;
        }

        /// <summary>
        /// befüllt eine noch leere DataTable
        /// </summary>
        /// <param name="tableID">ID, welche von 'AppendToDataList' vergeben wurde</param>
        /// <param name="information">die relevanten Daten ([0] = Position, [1] = Champion, [2] = Winrate, [3] = Games)</param>
        public static void AppendRowByID(Testetsstsset item)
        {
            //if (DataList.Count <= tableID)
            //    return;

            //DataTable dt = DataList[tableID];
            //if (information.Length != dt.Columns.Count)
            //    return;

            //DataRow row = dt.NewRow();

            //for (int i = 0; i < information.Length; i++)
            //{
            //    row[i] = information[i];
            //}

            //dt.Rows.Add(row);
            DataList.Add(item);
        }

        /// <summary>
        /// gibt einen Championnamen abhängig von der CID zurück
        /// </summary>
        /// <param name="cid">die ChampionID, welche von Lolalytics vergeben wurde und in 'SetChampions' gesettet wird</param>
        /// <returns></returns>
        public static string GetChampionByCID(int cid)
        {
            if (Champions == null || ChampionID == null || Champions.Count == 0 || ChampionID.Count == 0 || Champions.Count != ChampionID.Count)
                return "unknown";

            return Champions[ChampionID.IndexOf(cid.ToString())];
        }

        /// <summary>
        /// lädt die Champions und die ChampionIDs in Memory.Champions und Memory.ChampionID - die zugehörige ChampionID zu jedem Champion hat den gleichen Index in beiden Listen
        /// TODO: Die champions.json von Lolalytics mit der JSON-Package von Newtonsoft lesen (wie im 'Importer.cs')
        /// </summary>
        public static void SetChampions()
        {
            if (!Directory.Exists(Config.QueriesPath) || !Directory.Exists(RoleFolderQueriesPath))
                return;

            string championsRequestPath = Config.QueriesPath + "\\" + "Champions_" + Config.CurrentPatch + ".txt";
            string championsRequest = "";

            using (StreamReader reader = new StreamReader(championsRequestPath))
            {
                championsRequest = reader.ReadToEnd();
            }

            string[] championsSplit = championsRequest.Split(new string[] { "<h3>" }, StringSplitOptions.None);

            Champions = new List<string>(championsSplit.Length - 1);
            ChampionID = new List<string>(championsSplit.Length - 1);
            ChampionID.Add(championsSplit[0].TrimStart('"').Replace("\"", "").TrimEnd(':').Remove(0, 1));

            for (int i = 1; i <= championsSplit.Length - 2; i++)
            {
                string[] temp = championsSplit[i].Split(new string[] { "<\\/h3>" }, StringSplitOptions.None);
                Champions.Add(temp[0]);

                temp = temp[1].Split(new string[] { ",\"" }, StringSplitOptions.None);
                string cid = temp[1].TrimStart('"').Replace("\"", "").TrimEnd(':');
                ChampionID.Add(cid);
            }
            Champions.Add(championsSplit[championsSplit.Length - 1].Split(new string[] { "<\\/h3>" }, StringSplitOptions.None)[0]);

            Console.WriteLine("Lookup table with " + Champions.Count + " champions loaded.");
        }

        /// <summary>
        /// Setzt die beiden Dateipfade (hauptsächlich benutzt von 'ImporterAPI.cs', 'Importer.cs' und 'Loader.cs') abhängig von der Rolle in den UserSettings
        /// </summary>
        public static void SetRoleFolderPath()
        {
            switch (Config.CurrentRole)
            {
                case Constants.Top:
                    RoleFolderPath = Config.RawTopPath;
                    RoleFolderQueriesPath = Config.QueriesTopPath;
                    break;

                case Constants.Jungle:
                    RoleFolderPath = Config.RawJunglePath;
                    RoleFolderQueriesPath = Config.QueriesJunglePath;
                    break;

                case Constants.Middle:
                    RoleFolderPath = Config.RawMiddlePath;
                    RoleFolderQueriesPath = Config.QueriesMiddlePath;
                    break;

                case Constants.Bottom:
                    RoleFolderPath = Config.RawBottomPath;
                    RoleFolderQueriesPath = Config.QueriesBottomPath;
                    break;

                case Constants.Support:
                    RoleFolderPath = Config.RawSupportPath;
                    RoleFolderQueriesPath = Config.QueriesSupportPath;
                    break;

                default:
                    throw new Exception("Error at setting the role. " + Constants.RestartProgram);
            }
        }
    }
}