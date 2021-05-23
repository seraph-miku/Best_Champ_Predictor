using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Best_Champ_Predictor
{
    public class Lobby
    {
        public string OwnRole = Config.CurrentRole;

        public string EnemyTop = "";
        public string EnemyJungle = "";
        public string EnemyMid = "";
        public string EnemyBot = "";
        public string EnemySupp = "";
        public string OwnTop = "";
        public string OwnJungle = "";
        public string OwnMid = "";
        public string OwnBot = "";
        public string OwnSupp = "";
    }

    /// <summary>
    /// Printet eine Lobby, wo der User seine aktuelle Lobby eintippen kann. Dabei muss ein Kurzel für die Rolle verwendet werden + der Champion auf dieser Rolle. "you" belegt die eigene Rolle.
    /// et = EnemyTop, ej = EnemyJungle, em = EnemyMiddle, eb = EnemyBottom, es = EnemySupport, ot = OwnTop, oj = OwnJungle, om = OwnMiddle, ob = OwnBottom, os = OwnSupport
    /// </summary>
    internal class LobbySetter
    {
        public Lobby Lobby { get; set; } = new Lobby();

        public LobbySetter()
        {
            Console.WriteLine("Set your role:");
            SetOwnRole();

            bool leave = false;
            while (!leave)
            {
                Console.WriteLine();
                PrintLobbySeter();
                Console.WriteLine();
                Console.WriteLine("Set your lobby (e.g 'et renekton', 'exit' to go back to main menu):");
                var input = Console.ReadLine();

                if (input == "exit")
                {
                    leave = true;
                    return;
                }

                InterpretInput(input);

                Analyzer analyzer = new Analyzer(Lobby);
                Console.WriteLine();
                analyzer.ResultPrint();
            }
        }

        /// <summary>
        /// Befüllt die Liste 'Roles' mit den bekannten Champions in der Lobby, welche im Anschluss an den 'Analyzer.cs' weitergegeben wird
        /// </summary>
        //private void FillList()
        //{
        //    Roles.Add(OwnRole);
        //    Roles.Add(EnemyTop);
        //    Roles.Add(EnemyJungle);
        //    Roles.Add(EnemyMid);
        //    Roles.Add(EnemyBot);
        //    Roles.Add(EnemySupp);
        //    Roles.Add(OwnTop);
        //    Roles.Add(OwnJungle);
        //    Roles.Add(OwnMid);
        //    Roles.Add(OwnBot);
        //    Roles.Add(OwnSupp);
        //}

        /// <summary>
        /// Der Input, welcher vom User gegeben wurde (z.B. ist 'et renekton' = EnemyTop Renekton). Mit 'calc' verlässt man den LobbySetter und ruft den Analyzer.cs auf mit allen bekannten Champs.
        /// </summary>
        /// <param name="input">Input vom User</param>
        private void InterpretInput(string input)
        {
            string[] entries = input.Split(' ');

            if (entries.Length < 2)
            {
                Console.WriteLine("Could not interpret input. Not enough parameters.");
                return;
            }

            if (entries.Length > 2)
            {
                Console.WriteLine("Could not interpret input. Too many parameters.");
                return;
            }

            if (entries[1] == "clear")
                entries[1] = "";

            switch (entries[0])
            {
                case "et":
                    Lobby.EnemyTop = entries[1];
                    break;

                case "ej":
                    Lobby.EnemyJungle = entries[1];
                    break;

                case "em":
                    Lobby.EnemyMid = entries[1];
                    break;

                case "eb":
                    Lobby.EnemyBot = entries[1];
                    break;

                case "es":
                    Lobby.EnemySupp = entries[1];
                    break;

                case "ot":
                    Lobby.OwnTop = entries[1];
                    break;

                case "oj":
                    Lobby.OwnJungle = entries[1];
                    break;

                case "om":
                    Lobby.OwnMid = entries[1];
                    break;

                case "ob":
                    Lobby.OwnBot = entries[1];
                    break;

                case "os":
                    Lobby.OwnSupp = entries[1];
                    break;

                default:
                    Console.WriteLine("Could not interpret first parameter " + entries[0]);
                    return;
            }
        }

        /// <summary>
        /// Schreibt automatisch "you" zu der Rolle, welche in den UserSettings vermerkt ist
        /// </summary>
        private void SetOwnRole()
        {
            switch (Lobby.OwnRole)
            {
                case Constants.Top:
                    Lobby.OwnTop = "you";
                    break;

                case Constants.Jungle:
                    Lobby.OwnJungle = "you";
                    break;

                case Constants.Middle:
                    Lobby.OwnMid = "you";
                    break;

                case Constants.Bottom:
                    Lobby.OwnBot = "you";
                    break;

                case Constants.Support:
                    Lobby.OwnSupp = "you";
                    break;

                default:
                    throw new Exception("Error at setting the role in Lobby Setter. " + Constants.RestartProgram);
            }
        }

        /// <summary>
        /// Printet die aktuelle Lobby
        /// </summary>
        private void PrintLobbySeter()
        {
            Console.WriteLine("EnemyTop    = " + Lobby.EnemyTop);
            Console.WriteLine("EnemyJungle = " + Lobby.EnemyJungle);
            Console.WriteLine("EnemyMid    = " + Lobby.EnemyMid);
            Console.WriteLine("EnemyBot    = " + Lobby.EnemyBot);
            Console.WriteLine("EnemySupp   = " + Lobby.EnemySupp);
            Console.WriteLine("OwnTop      = " + Lobby.OwnTop);
            Console.WriteLine("OwnJungle   = " + Lobby.OwnJungle);
            Console.WriteLine("OwnMid      = " + Lobby.OwnMid);
            Console.WriteLine("OwnBot      = " + Lobby.OwnBot);
            Console.WriteLine("OwnSupp     = " + Lobby.OwnSupp);
        }
    }
}