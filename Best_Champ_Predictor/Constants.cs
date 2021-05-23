using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Best_Champ_Predictor
{
    /// <summary>
    /// Konstanten im Programm, die man benutzen kann, damit man sich nicht vertippt
    /// </summary>
    public static class Constants
    {
        //Program Constants

        public const string RestartProgram = "Please restart the program.";
        public const string Cancelled = "Cancelled!";
        public const string NoDataFound = "No imported Data found. Please import Data from Lolalytics first.";
        public const string CorruptedData = "Data corrupted. Please import Data from Lolalytics again.";

        //Config

        public const string CurrentPatch = "Current Patch";
        public const string GetPatch = "Get Data from";
        public const string CurrentRole = "Set Role";
        public const string EloFilter = "Elo Filter";
        public const string Region = "Set Region";
        public const string NumberOfGames = "Filter min. games";
        public const string ResetDefault = "Reset default";
        public const string SettingsCorrupted = "User settings corrupted. Default restored.";

        //LoL Constants

        public const string Top = "top";
        public const string Jungle = "jungle";
        public const string Middle = "middle";
        public const string Bottom = "bottom";
        public const string Support = "support";
        public const string EnemyTop = "enemyTop";
        public const string EnemyJungle = "enemyJungle";
        public const string EnemyMiddle = "enemyMiddle";
        public const string EnemyBottom = "enemyBottom";
        public const string EnemySupport = "enemySupport";
        public const string OwnTop = "ownTop";
        public const string OwnJungle = "ownJungle";
        public const string OwnMiddle = "ownMiddle";
        public const string OwnBottom = "ownBottom";
        public const string OwnSupport = "ownSupport";
    }
}