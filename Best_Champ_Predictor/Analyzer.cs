using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Best_Champ_Predictor.Importer;
using static Best_Champ_Predictor.Importer.Testetsstsset;

namespace Best_Champ_Predictor
{
    public class WInrateFFOrChamp
    {
        public String Champion { get; set; }
        public String Position { get; set; }
        public Double AVRWR { get; set; }
        public List<WinrrateStats> WinrrateStats { get; set; } = new List<WinrrateStats>();
    }

    public class WinrrateStats
    {
        public String Position { get; set; }
        public String Champion { get; set; }
        public Double WR { get; set; }
        public Int32 Games { get; set; }
    }

    /// <summary>
    /// Analysiert die weitergegebenen Daten vom 'LobbySetter.cs' und gibt für jeden Champion in der ausgewählten Rolle eine Gewinnwahrscheinlichkeit zurück.
    /// Die Champions mit der höchsten Gewinnwahrscheinlichkeit stehen ganz oben.
    /// </summary>
    internal class Analyzer
    {
        //Liste, welche von die relevanten DataTables aus 'Memory.DataList' abgreift (abhängig von den Eingaben aus 'LobbySetter.cs'). Wird nach dem Abgreifen an den Analysealgorithmus übergeben.
        private List<WInrateFFOrChamp> Statistics = new List<WInrateFFOrChamp>();

        //Abschließende Endtabelle, welcher der User nach der Analyse sieht
        public DataTable Result = new DataTable();

        public Analyzer(Lobby lobby)
        {
            FillStatistics(lobby);
            CalcStatistics(lobby);
            SortByWinrate();
        }

        /// <summary>
        /// Berechnet die endgültige Winrate von jedem Champion abhängig von der Rolle und den Eingaben im 'LobbySetter.cs'.
        /// Dabei wird eine 'shareAVGWinrate' durch den 'ShareCalculator.cs' berechnet (abhängig von der eigenen Rolle und davon wie viele unbekannte Positionen es gibt).
        /// Da jede Position eine unterschiedliche Gewichtung auf die eigene Rolle hat (z.B. wenn man Top ist, so ist es wichtiger zu wissen wer enemyTop ist und nicht wer enemySupp ist)
        /// wird die berechnete 'shareAVGWinrate' am Ende mit der 'avgWinrate' (durschnittliche Winrate von einem Champion gegen einen noch unbekannten Gegner) multipliziert.
        /// Bei bekannten Gegnern wird eine Gewichtung vom 'ShareCalculator.cs' für die jeweilige Rolle vergeben. Trotzdem spielt bei bekannten Gegner ein Anteil von der 'avgWinrate' mit,
        /// da bei Gegnern mit wenigen bekannten Spielen die Winrate einen kleineren Anteil haben sollte (da eine geringe Sample Size eine große Varianz hat). Die genaue Formel für
        /// die Berechnung der beiden Anteile bei bekannten Gegnern ist im 'ShareCalculatorFunction.PNG' zu sehen.
        /// </summary>
        /// <param name="roles">Champions, die im 'LobbySetter.cs' gepicked wurden</param>
        private void CalcStatistics(Lobby lobby)
        {
            if (Statistics.Count == 0)
                return;

            Result.Columns.Add("Champion", typeof(string));
            Result.Columns.Add("Predicted_WR", typeof(double));
            Result.Columns.Add("Games_Analyzed", typeof(int));

            double shareAVGWinrate = ShareCalculator.AVGWinrateShareBasedOnEmptyLanes(lobby);

            foreach (var dt in Statistics)
            {
                //string[] splitInformation = dt.TableName.Split(new String[] { " - " }, StringSplitOptions.None);
                //string champName = splitInformation[0];
                //double avgWinrate = Convert.ToDouble(splitInformation[1]);
                //int totalGames = Convert.ToInt32(splitInformation[2]);

                DataRow resultRow = Result.NewRow();
                int totalPlayed = 0;
                double predictedWR = 0;
                double matchupWinrate = 0d;

                foreach (var row in dt.WinrrateStats)
                {
                    totalPlayed += row.Games;

                    double thisRowImpactShare = ShareCalculator.ShareBasedOnLane(dt.Position, (string)row.Position);
                    int matchupGames = row.Games;
                    double thisRowMatchupShare = ShareCalculator.ShareBasedOnGames(matchupGames);
                    double thisRowStandardWinrateShare = 1 - thisRowMatchupShare;

                    matchupWinrate += (((double)row.WR * thisRowMatchupShare) + (dt.AVRWR * thisRowStandardWinrateShare)) * thisRowImpactShare;
                }
                if (matchupWinrate == 0)
                    predictedWR = dt.AVRWR;
                else
                    predictedWR = (shareAVGWinrate * dt.AVRWR) + matchupWinrate;

                resultRow[0] = dt.Champion;
                resultRow[1] = predictedWR;
                resultRow[2] = totalPlayed;
                Result.Rows.Add(resultRow);
            }
        }

        /// <summary>
        /// Sortiert die Result-Tabelle nach der Winrate absteigend, sodass der beste Pick ganz oben steht.
        /// </summary>
        private void SortByWinrate()
        {
            if (Result.Rows.Count == 0)
                return;

            DataView dv = Result.DefaultView;
            dv.Sort = "Predicted_WR desc";
            Result = dv.ToTable();
        }

        /// <summary>
        /// Gibt zurück, ob der Champ mit dem Namen 'champName' bereits gepicked wurde, um zu verhindern, dass das Programm z.B. vorschlägt Janna zu picken, obwohl der Gegner schon Janna gepicked hat.
        /// </summary>
        /// <param name="champName">Name des Champions, bei dem geuckt wird, ob er bereits gepicked wurde.</param>
        /// <param name="lobbyChamps">Alle Champions, die in der Lobby bereits gepicked wurden.</param>
        /// <returns>Gibt zurück, ob der Champ mit dem Namen 'champName' bereits gepicked wurde</returns>
        private bool ChampAlreadyPicked(string champName, Lobby lobby)
        {
            //for (int i = 1; i < lobbyChamps.Count; i++)
            //{
            //    if (lobbyChamps[i] == champName.ToLower())
            //        return true;
            //}
            if (lobby.OwnTop == champName)
                return true;
            if (lobby.OwnJungle == champName)
                return true;
            if (lobby.OwnMid == champName)
                return true;
            if (lobby.OwnBot == champName)
                return true;
            if (lobby.OwnSupp == champName)
                return true;
            if (lobby.EnemyTop == champName)
                return true;
            if (lobby.EnemyJungle == champName)
                return true;
            if (lobby.EnemyMid == champName)
                return true;
            if (lobby.EnemyBot == champName)
                return true;
            if (lobby.EnemySupp == champName)
                return true;
            return false;
        }

        /// <summary>
        /// Befüllt die Statistics-Liste mit den relevanten DataTables (abhängig von den Eingaben im 'LobbySetter.cs'). Ignoriert DataTable, bei denen der Champion weniger Games als
        /// die vom User gesetzte 'Config.MinNumberOfGames' aufweist. Ignoriert Champions, die bereits von anderen Spielern im 'LobbySetter.cs' gepicked wurden.
        /// </summary>
        /// <param name="roles">Champions, die im 'LobbySetter.cs' gepicked wurden</param>
        private void FillStatistics(Lobby lobby)
        {
            List<Testetsstsset> effectiveDataList = new List<Testetsstsset>();
            for (int i = 0; i < Memory.DataList.Count; i++)
            {
                var item = Memory.DataList[i];

                //string[] splitInformation = Memory.DataList[i].TableName.Split(new String[] { " - " }, StringSplitOptions.None);
                //string roleName = splitInformation[0];
                //string champName = splitInformation[1];
                //double avgWinrate = Convert.ToDouble(splitInformation[2]);
                //int totalGames = Convert.ToInt32(splitInformation[3]);

                if (item.OwnRole == Config.CurrentRole && item.GameCount > Config.MinNumberOfGames && !ChampAlreadyPicked(item.OwncHamp, lobby))
                    effectiveDataList.Add(item);
            }

            foreach (Testetsstsset dt in effectiveDataList)
            {
                //string[] splitInformation = dt.TableName.Split(new String[] { " - " }, StringSplitOptions.None);
                //string roleName = splitInformation[0];
                //string champName = splitInformation[1];
                //double avgWinrate = Convert.ToDouble(splitInformation[2]);
                //int totalGames = Convert.ToInt32(splitInformation[3]);
                WInrateFFOrChamp winrateGames = new WInrateFFOrChamp();
                winrateGames.AVRWR = dt.AvgWr;
                winrateGames.Champion = dt.OwncHamp;
                winrateGames.Position = Config.CurrentRole;

                //DataTable winrateGames = new DataTable();
                //winrateGames.TableName = champName + " - " + avgWinrate + " - " + totalGames;
                //winrateGames.Columns.Add("Position", typeof(string));
                //winrateGames.Columns.Add("WR", typeof(double));
                //winrateGames.Columns.Add("Games", typeof(int));

                if (lobby.EnemyTop != "")
                {
                    ChampionWinrates item = dt.Enemy.TOP.SingleOrDefault(x => (x.Champion.ToLower().Replace(" ", "")) == lobby.EnemyTop);
                    if (item != null)
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = item.WR,
                            Games = item.Games,
                            Champion = item.Champion,
                            Position = Constants.EnemyTop
                        });
                    }
                    else
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = dt.AvgWr,
                            Games = 0,
                            Champion = lobby.EnemyTop,
                            Position = Constants.EnemyTop
                        });
                    }
                }
                if (lobby.EnemyJungle != "")
                {
                    ChampionWinrates item = dt.Enemy.JGL.SingleOrDefault(x => (x.Champion.ToLower().Replace(" ", "")) == lobby.EnemyJungle);
                    if (item != null)
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = item.WR,
                            Games = item.Games,
                            Champion = item.Champion,
                            Position = Constants.EnemyJungle
                        });
                    }
                    else
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = dt.AvgWr,
                            Games = 0,
                            Champion = lobby.EnemyJungle,
                            Position = Constants.EnemyJungle
                        });
                    }
                }
                if (lobby.EnemyMid != "")
                {
                    ChampionWinrates item = dt.Enemy.MID.SingleOrDefault(x => (x.Champion.ToLower().Replace(" ", "")) == lobby.EnemyMid);
                    if (item != null)
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = item.WR,
                            Games = item.Games,
                            Champion = item.Champion,
                            Position = Constants.EnemyMiddle
                        });
                    }
                    else
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = dt.AvgWr,
                            Games = 0,
                            Champion = lobby.EnemyMid,
                            Position = Constants.EnemyMiddle
                        });
                    }
                }
                if (lobby.EnemyBot != "")
                {
                    ChampionWinrates item = dt.Enemy.BoT.SingleOrDefault(x => (x.Champion.ToLower().Replace(" ", "")) == lobby.EnemyBot);
                    if (item != null)
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = item.WR,
                            Games = item.Games,
                            Champion = item.Champion,
                            Position = Constants.EnemyBottom
                        });
                    }
                    else
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = dt.AvgWr,
                            Games = 0,
                            Champion = lobby.EnemyBot,
                            Position = Constants.EnemyBottom
                        });
                    }
                }
                if (lobby.EnemySupp != "")
                {
                    ChampionWinrates item = dt.Enemy.SUp.SingleOrDefault(x => (x.Champion.ToLower().Replace(" ", "")) == lobby.EnemySupp);
                    if (item != null)
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = item.WR,
                            Games = item.Games,
                            Champion = item.Champion,
                            Position = Constants.EnemySupport
                        });
                    }
                    else
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = dt.AvgWr,
                            Games = 0,
                            Champion = lobby.EnemySupp,
                            Position = Constants.EnemySupport
                        });
                    }
                }
                if (lobby.OwnTop != "")
                {
                    ChampionWinrates item = dt.Own.TOP.SingleOrDefault(x => (x.Champion.ToLower().Replace(" ", "")) == lobby.OwnTop);
                    if (item != null)
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = item.WR,
                            Games = item.Games,
                            Champion = item.Champion,
                            Position = Constants.OwnTop
                        });
                    }
                    else
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = dt.AvgWr,
                            Games = 0,
                            Champion = lobby.OwnTop,
                            Position = Constants.OwnTop
                        });
                    }
                }
                if (lobby.OwnJungle != "")
                {
                    ChampionWinrates item = dt.Own.JGL.SingleOrDefault(x => (x.Champion.ToLower().Replace(" ", "")) == lobby.OwnJungle);
                    if (item != null)
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = item.WR,
                            Games = item.Games,
                            Champion = item.Champion,
                            Position = Constants.OwnJungle
                        });
                    }
                    else
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = dt.AvgWr,
                            Games = 0,
                            Champion = lobby.OwnJungle,
                            Position = Constants.OwnJungle
                        });
                    }
                }
                if (lobby.OwnMid != "")
                {
                    ChampionWinrates item = dt.Own.MID.SingleOrDefault(x => (x.Champion.ToLower().Replace(" ", "")) == lobby.OwnMid);
                    if (item != null)
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = item.WR,
                            Games = item.Games,
                            Champion = item.Champion,
                            Position = Constants.OwnMiddle
                        });
                    }
                    else
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = dt.AvgWr,
                            Games = 0,
                            Champion = lobby.OwnMid,
                            Position = Constants.OwnMiddle
                        });
                    }
                }
                if (lobby.OwnBot != "")
                {
                    ChampionWinrates item = dt.Own.BoT.SingleOrDefault(x => (x.Champion.ToLower().Replace(" ", "")) == lobby.OwnBot);
                    if (item != null)
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = item.WR,
                            Games = item.Games,
                            Champion = item.Champion,
                            Position = Constants.OwnBottom
                        });
                    }
                    else
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = dt.AvgWr,
                            Games = 0,
                            Champion = lobby.OwnBot,
                            Position = Constants.OwnBottom
                        });
                    }
                }
                if (lobby.OwnSupp != "")
                {
                    ChampionWinrates item = dt.Own.SUp.SingleOrDefault(x => (x.Champion.ToLower().Replace(" ", "")) == lobby.OwnSupp);
                    if (item != null)
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = item.WR,
                            Games = item.Games,
                            Champion = item.Champion,
                            Position = Constants.OwnSupport
                        });
                    }
                    else
                    {
                        winrateGames.WinrrateStats.Add(new WinrrateStats()
                        {
                            WR = dt.AvgWr,
                            Games = 0,
                            Champion = lobby.OwnSupp,
                            Position = Constants.OwnSupport
                        });
                    }
                }

                Statistics.Add(winrateGames);
            }
        }

        /// <summary>
        /// Printet die Result-Table
        /// </summary>
        public void ResultPrint()
        {
            int maxCharsChamp = 0;
            int maxCharsWR = 0;
            int maxCharsGames = 0;

            for (int i = 0; i < Result.Rows.Count; i++)
            {
                if (maxCharsChamp < Result.Rows[i][0].ToString().Length)
                    maxCharsChamp = Result.Rows[i][0].ToString().Length;

                if (maxCharsWR < Math.Round((double)Result.Rows[i][1], 2).ToString().Length)
                    maxCharsWR = Math.Round((double)Result.Rows[i][1], 2).ToString().Length;

                if (maxCharsGames < Result.Rows[i][2].ToString().Length)
                    maxCharsGames = Result.Rows[i][2].ToString().Length;
            }

            Console.WriteLine("Ranking:");
            for (int i = 0; i < Result.Rows.Count; i++)
            {
                string spacingFirst = "";
                if (i < 9)
                    spacingFirst = "  ";
                else if (i < 99)
                    spacingFirst = " ";

                string spacingSecond = " ";
                for (int j = Result.Rows[i][0].ToString().Length; j < maxCharsChamp; j++)
                    spacingSecond += " ";

                string spacingThird = " ";
                for (int j = Math.Round((double)Result.Rows[i][1], 2).ToString().Length; j < maxCharsWR; j++)
                    spacingThird += " ";

                string spacingFourth = " ";
                for (int j = Result.Rows[i][2].ToString().Length; j < maxCharsGames; j++)
                    spacingFourth += " ";

                Console.WriteLine((i + 1) + ")" + spacingFirst + Result.Rows[i][0] + spacingSecond + Math.Round((double)Result.Rows[i][1], 2) + "%" + spacingThird + "over " + Result.Rows[i][2] + spacingFourth + "games");
            }
        }
    }
}