using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Best_Champ_Predictor.Importer;

namespace Best_Champ_Predictor.Import
{
    public static class Statichelperclassse
    {
        public static List<Testetsstsset.ChampionWinrates> ToChampionWinrates(this List<List<double>> champ)
        {
            if (champ == null)
                return new List<Testetsstsset.ChampionWinrates>();

            return champ?.Select(x => new Testetsstsset.ChampionWinrates()
            {
                Champion = Memory.GetChampionByCID((int)x[0]),
                Games = (int)x[1],
                WR = Math.Round(((double)x[2] / (double)x[1]) * 100, 2),
            }).ToList();
        }
    }
}