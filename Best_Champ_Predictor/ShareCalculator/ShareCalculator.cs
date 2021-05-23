using System;
using System.Collections.Generic;
using System.Data;

namespace Best_Champ_Predictor
{
    public static class ShareCalculator
    {
        #region ImpactByOtherLanesBasedOnRole

        //Gewichtung von anderen Spielern auf die Winrateberechnung abhängig von der eigenen Rolle
        //[0]enemyTop, [1]enemyJungle, [2]enemyMid, [3]enemyBot, [4]enemySupp,
        //[5]ownTop, [6]ownJungle, [7]ownMid, [8]ownBot, [9]ownSupp

        private static double[] topImpact = new double[10] { 0.35d, 0.15d, 0.075d, 0.05d, 0.05d, 0d, 0.15d, 0.075d, 0.05d, 0.05d };
        private static double[] jungleImpact = new double[10] { 0.1d, 0.2d, 0.1d, 0.1d, 0.1d, 0.1d, 0d, 0.1d, 0.1d, 0.1d };
        private static double[] midImpact = new double[10] { 0.05d, 0.2d, 0.3d, 0.05d, 0.05d, 0.05d, 0.2d, 0d, 0.05d, 0.05d };
        private static double[] botImpact = new double[10] { 0.04d, 0.12d, 0.08d, 0.2d, 0.16d, 0.04d, 0.12d, 0.08d, 0d, 0.16d };
        private static double[] suppImpact = new double[10] { 0.04d, 0.12d, 0.08d, 0.16d, 0.2d, 0.04d, 0.08d, 0.12d, 0.16d, 0d };

        #endregion ImpactByOtherLanesBasedOnRole

        /// <summary>
        /// Berechnet den Anteil der Standard-Winrate, welcher abhängig von den unbekannten Lanes ist und von der eigenen Rolle (da jede Rolle eine eigene Gewichtung hat - s.o. die ImpactArrays)
        /// </summary>
        /// <param name="roles">Champions, die im 'LobbySetter.cs' gepicked wurden</param>
        /// <returns>den Anteil von der Standard-Winrate</returns>
        public static double AVGWinrateShareBasedOnEmptyLanes(Lobby lobby)
        {
            double[] activeImpactArray = new double[10];
            double inverseAVGWinrateShare = 0;

            GetImpactArray(Config.CurrentRole).CopyTo(activeImpactArray, 0);

            if (lobby.EnemyTop != "")
                inverseAVGWinrateShare += activeImpactArray[0];
            if (lobby.EnemyJungle != "")
                inverseAVGWinrateShare += activeImpactArray[1];
            if (lobby.EnemyMid != "")
                inverseAVGWinrateShare += activeImpactArray[2];
            if (lobby.EnemyBot != "")
                inverseAVGWinrateShare += activeImpactArray[3];
            if (lobby.EnemySupp != "")
                inverseAVGWinrateShare += activeImpactArray[4];
            if (lobby.OwnTop != "")
                inverseAVGWinrateShare += activeImpactArray[5];
            if (lobby.OwnJungle != "")
                inverseAVGWinrateShare += activeImpactArray[6];
            if (lobby.OwnMid != "")
                inverseAVGWinrateShare += activeImpactArray[7];
            if (lobby.OwnBot != "")
                inverseAVGWinrateShare += activeImpactArray[8];
            if (lobby.OwnSupp != "")
                inverseAVGWinrateShare += activeImpactArray[9];

            return 1 - inverseAVGWinrateShare;
        }

        /// <summary>
        /// Rechnet die Gewictung für ein Matchup aus, abhängig von der eigenen Positions und der Positions des Matchups
        /// </summary>
        /// <param name="ownPosition">eigene Position, wie in den UserSettings angegeben</param>
        /// <param name="matchupPosition">Position des Matchups (welche von User im 'LobbySetter.cs' gesetzt wurde</param>
        /// <returns>die Gewichtung</returns>
        public static double ShareBasedOnLane(string ownPosition, string matchupPosition)
        {
            int matchupRoleIndex = RoleToIndex(matchupPosition);
            double shareResult = GetImpactArray(ownPosition)[matchupRoleIndex];
            return shareResult;
        }

        /// <summary>
        /// Graphische Darstellung der Funktion zu finden in der Datei 'ShareCalculatorFuction.PNG' im Ordner 'ShareCalculator'.
        /// x ist dabei die Anzahl der Spiele, y der Anteil was für eine Gewichtung das Matchup bekommen soll (der Rest ist die avgWinrate des Champions)
        /// </summary>
        /// <param name="gamesCount">Anzahl der Spiele in einem Matchup</param>
        /// <returns>Anteil, wie stark das Matchup für die eine Lane gewichtet werden soll (y-Wert im Graphen)</returns>
        public static double ShareBasedOnGames(int gamesCount)
        {
            double b = -4.2d;
            double c = 30d;
            double result = 1d / (1d + Math.Exp((b * (Math.Log10(gamesCount / c)))));
            return result;
        }

        /// <summary>
        /// Holt sich den relevanten GewichtungsArray, abhängig von der eigenen Rolle
        /// </summary>
        /// <param name="ownRole">Rolle, die in den UserSettings angegeben wurde</param>
        /// <returns>den notwendigen ImpactArray</returns>
        private static double[] GetImpactArray(string ownRole)
        {
            switch (ownRole)
            {
                case Constants.Top:
                    return topImpact;

                case Constants.Jungle:
                    return jungleImpact;

                case Constants.Middle:
                    return midImpact;

                case Constants.Bottom:
                    return botImpact;

                case Constants.Support:
                    return suppImpact;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Gibt einen Index zurück, welcher dann dafür benutzt werden kann, um die richtige Gewichtung im activeImpactArray zu finden
        /// </summary>
        /// <param name="role">Rolle des Matchups (nicht die eigene Rolle)</param>
        /// <returns>Index, welcher im activeImpactArray representativ für die Rolle ist</returns>
        private static int RoleToIndex(string role)
        {
            switch (role)
            {
                case Constants.EnemyTop:
                    return 0;

                case Constants.EnemyJungle:
                    return 1;

                case Constants.EnemyMiddle:
                    return 2;

                case Constants.EnemyBottom:
                    return 3;

                case Constants.EnemySupport:
                    return 4;

                case Constants.OwnTop:
                    return 5;

                case Constants.OwnJungle:
                    return 6;

                case Constants.OwnMiddle:
                    return 7;

                case Constants.OwnBottom:
                    return 8;

                case Constants.OwnSupport:
                    return 9;

                default:
                    throw new Exception("Error at interpreting role.");
            }
        }
    }
}