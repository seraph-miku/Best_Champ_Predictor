using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Best_Champ_Predictor.Importer;

namespace Best_Champ_Predictor
{
    /// <summary>
    /// Lädt die Daten zu jedem Champion (falls vorhanden) zur aktuell gesetzten Rolle aus dem Ordner 'Memory.RoleFolderPath' in die 'Memory.DataList'
    /// TODO: FileChamp, FileWR, FileGames, CountEntries evlt. nicht als Klassenvariablen machen
    /// </summary>
    internal class Loader
    {
        public string Seperator = Config.Seperator;
        public string FileRole = Config.CurrentRole;

        //public string FileChamp = "";
        //public double FileWR = 0d;
        //public int FileGames = 0;
        //public int CountEntries = 0;

        /// <summary>
        /// Lädt die Daten zu jedem Champion (falls vorhanden) zur aktuell gesetzten Rolle aus dem Ordner 'Memory.RoleFolderPath' in die 'Memory.DataList'
        /// </summary>
        /// <param name="success">Gibt zurück, ob der Loader die Daten erfolgreich in den Arbeitsspeicher laden konnte</param>
        public Loader(out bool success)
        {
            success = false;

            if (!Directory.Exists(Config.QueriesPath) || !Directory.Exists(Memory.RoleFolderQueriesPath) || !Directory.Exists(Memory.RoleFolderPath))
            {
                Console.WriteLine(Constants.NoDataFound);
                return;
            }

            DirectoryInfo di = new DirectoryInfo(Memory.RoleFolderPath);
            FileInfo[] FilesLoad = di.GetFiles("*.txt");

            if (FilesLoad.Length == 0)
            {
                Console.WriteLine(Constants.NoDataFound);
                return;
            }

            foreach (FileInfo file in FilesLoad)
            {
                //FileChamp = file.Name.Split(new String[] { ".txt" }, StringSplitOptions.None)[0];
                //GetFileProperties(file.FullName);
                LoadInMemory(file.FullName);
                Console.WriteLine(file.FullName);
                //Console.WriteLine(FileRole + ", " + FileChamp + " loaded - " + CountEntries + " entries");
                //CountEntries = 0;
            }

            success = true;
        }

        /// <summary>
        /// Liest die erste Zeile aus 'filePath' und speichert die Winrate und die Games des dazugehörigen Champions zwischenzeitlich ab
        /// </summary>
        /// <param name="filePath">Pfad der Datei zum Champion, dessen Daten gerade in 'Memory.DataTable' abgespeichert werden</param>
        //public void GetFileProperties(string filePath)
        //{
        //    using (StreamReader reader = new StreamReader(filePath))
        //    {
        //        string firstLine = reader.ReadLine();
        //        string[] properties = firstLine.Split(new String[] { Seperator }, StringSplitOptions.None);
        //        FileWR = Convert.ToDouble(properties[0]);
        //        FileGames = Convert.ToInt32(properties[1]);
        //    }
        //}

        /// <summary>
        /// Nachdem eine neue leere DataTable an die 'Memory.DataList' drangehängt wurde (mit den Werten, die von 'GetFileProperties' gelesen wurden),
        /// wird ab der zweiten Zeile im 'filePath' weitergelesen und die leere DataTable mit allen relevanten Daten befüllt
        /// </summary>
        /// <param name="filePath">Pfad der Datei zum Champion, dessen Daten gerade in 'Memory.DataTable' abgespeichert werden</param>
        public void LoadInMemory(string filePath)
        {
            //Memory.AppendToDataList(FileRole, FileChamp, FileWR, FileGames, out int tableID);

            using (StreamReader reader = new StreamReader(filePath))
            {
                Testetsstsset myDeserializedClass = JsonConvert.DeserializeObject<Testetsstsset>(reader.ReadToEnd());
                //reader.ReadLine();
                //while (!reader.EndOfStream)
                //{
                //    string newLine = reader.ReadLine();

                //    if (newLine == "")
                //        continue;

                //    string[] information = newLine.Split(new String[] { Seperator }, StringSplitOptions.None);
                Memory.AppendRowByID(myDeserializedClass);
                //    CountEntries++;
                //}
            }
        }
    }
}