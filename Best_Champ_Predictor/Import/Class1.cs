using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Best_Champ_Predictor.Import
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Damage
    {
        public double physical { get; set; }
        public double magic { get; set; }
        public double @true { get; set; }
    }

    public class Header
    {
        public int n { get; set; }
        public string defaultLane { get; set; }
        public string lane { get; set; }
        public double wr { get; set; }
        public double pr { get; set; }
        public double br { get; set; }
        public string rank { get; set; }
        public string rankTotal { get; set; }
        public string tier { get; set; }
        public double topWin { get; set; }
        public string topElo { get; set; }
        public Damage damage { get; set; }
    }

    public class Wr
    {
        public List<double> all { get; set; }
        public List<double> diamond_plus { get; set; }
        public List<double> platinum { get; set; }
        public List<double> gold { get; set; }
        public List<double> silver { get; set; }
        public List<double> bronze { get; set; }
        public List<double> iron { get; set; }
    }

    public class Wrs
    {
        public List<double> all { get; set; }
        public List<double> diamond_plus { get; set; }
        public List<double> platinum { get; set; }
        public List<double> gold { get; set; }
        public List<double> silver { get; set; }
        public List<double> bronze { get; set; }
        public List<double> iron { get; set; }
    }

    public class Pr
    {
        public List<double> all { get; set; }
        public List<double> diamond_plus { get; set; }
        public List<double> platinum { get; set; }
        public List<double> gold { get; set; }
        public List<double> silver { get; set; }
        public List<double> bronze { get; set; }
        public List<double> iron { get; set; }
    }

    public class N
    {
        public List<int> all { get; set; }
        public List<int> diamond_plus { get; set; }
        public List<int> platinum { get; set; }
        public List<int> gold { get; set; }
        public List<int> silver { get; set; }
        public List<int> bronze { get; set; }
        public List<int> iron { get; set; }
    }

    public class Br
    {
        public List<double> all { get; set; }
        public List<double> diamond_plus { get; set; }
        public List<double> platinum { get; set; }
        public List<double> gold { get; set; }
        public List<double> silver { get; set; }
        public List<double> bronze { get; set; }
        public List<double> iron { get; set; }
    }

    public class Graph
    {
        public List<string> dates { get; set; }
        public Wr wr { get; set; }
        public Wrs wrs { get; set; }
        public Pr pr { get; set; }
        public N n { get; set; }
        public Br br { get; set; }
    }

    public class Lanes
    {
        public double top { get; set; }
        public double jungle { get; set; }
        public double middle { get; set; }
        public double bottom { get; set; }
        public double support { get; set; }
    }

    public class Nav
    {
        public Lanes lanes { get; set; }
    }

    public class Skills
    {
        public List<List<List<int>>> skillEarly { get; set; }
        public List<List<object>> skillOrder { get; set; }
        public List<List<int>> skill6 { get; set; }
        public int skill6Pick { get; set; }
        public List<List<int>> skill10 { get; set; }
        public int skill10Pick { get; set; }
        public List<List<object>> skill15 { get; set; }
        public int skill15Pick { get; set; }
    }

    public class Time
    {
        public int _4 { get; set; }
        public int _3 { get; set; }
        public int _2 { get; set; }
        public int _1 { get; set; }
        public int _6 { get; set; }
        public int _5 { get; set; }
    }

    public class TimeWin
    {
        public int _4 { get; set; }
        public int _2 { get; set; }
        public int _3 { get; set; }
        public int _6 { get; set; }
        public int _5 { get; set; }
    }

    public class TopStats
    {
        public int toppick { get; set; }
        public int toprank { get; set; }
        public int topcount { get; set; }
        public double topwin { get; set; }
        public string topelo { get; set; }
    }

    public class Stats
    {
        public List<List<int>> _8005 { get; set; }
        public List<List<int>> _8008 { get; set; }
        public List<List<int>> _8021 { get; set; }
        public List<List<double>> _8010 { get; set; }
        public List<List<int>> _9101 { get; set; }
        public List<List<double>> _9111 { get; set; }
        public List<List<int>> _8009 { get; set; }
        public List<List<double>> _9104 { get; set; }
        public List<List<double>> _9105 { get; set; }
        public List<List<double>> _9103 { get; set; }
        public List<List<double>> _8014 { get; set; }
        public List<List<int>> _8017 { get; set; }
        public List<List<double>> _8299 { get; set; }
        public List<List<int>> _8112 { get; set; }
        public List<List<int>> _8124 { get; set; }
        public List<List<int>> _8128 { get; set; }
        public List<List<int>> _9923 { get; set; }
        public List<List<double>> _8126 { get; set; }
        public List<List<double>> _8139 { get; set; }
        public List<List<double>> _8143 { get; set; }
        public List<List<int>> _8136 { get; set; }
        public List<List<int>> _8120 { get; set; }
        public List<List<int>> _8138 { get; set; }
        public List<List<double>> _8135 { get; set; }
        public List<List<int>> _8134 { get; set; }
        public List<List<double>> _8105 { get; set; }
        public List<List<double>> _8106 { get; set; }
        public List<List<int>> _8214 { get; set; }
        public List<List<double>> _8229 { get; set; }
        public List<List<int>> _8230 { get; set; }
        public List<List<double>> _8224 { get; set; }
        public List<List<int>> _8226 { get; set; }
        public List<List<int>> _8275 { get; set; }
        public List<List<int>> _8210 { get; set; }
        public List<List<double>> _8234 { get; set; }
        public List<List<int>> _8233 { get; set; }
        public List<List<double>> _8237 { get; set; }
        public List<List<int>> _8232 { get; set; }
        public List<List<int>> _8236 { get; set; }
        public List<List<int>> _8437 { get; set; }
        public List<List<double>> _8439 { get; set; }
        public List<List<int>> _8465 { get; set; }
        public List<List<double>> _8446 { get; set; }
        public List<List<double>> _8463 { get; set; }
        public List<List<int>> _8401 { get; set; }
        public List<List<int>> _8429 { get; set; }
        public List<List<double>> _8444 { get; set; }
        public List<List<double>> _8473 { get; set; }
        public List<List<int>> _8451 { get; set; }
        public List<List<double>> _8453 { get; set; }
        public List<List<double>> _8242 { get; set; }
        public List<List<int>> _8351 { get; set; }
        public List<List<int>> _8360 { get; set; }
        public List<List<int>> _8358 { get; set; }
        public List<List<int>> _8306 { get; set; }
        public List<List<int>> _8304 { get; set; }
        public List<List<int>> _8313 { get; set; }
        public List<List<int>> _8321 { get; set; }
        public List<List<int>> _8316 { get; set; }
        public List<List<int>> _8345 { get; set; }
        public List<List<int>> _8347 { get; set; }
        public List<List<int>> _8410 { get; set; }
        public List<List<int>> _8352 { get; set; }
        public List<List<double>> _5008 { get; set; }
        public List<List<double>> _5005 { get; set; }
        public List<List<double>> _5007 { get; set; }
        public List<List<double>> _5008f { get; set; }
        public List<List<double>> _5002f { get; set; }
        public List<List<int>> _5003f { get; set; }
        public List<List<int>> _5001 { get; set; }
        public List<List<double>> _5002 { get; set; }
        public List<List<double>> _5003 { get; set; }
    }

    public class Runes
    {
        public Stats stats { get; set; }
    }

    public class Blue
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class Rift1
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class Rift2
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class RiftDouble
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class EliteDouble
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class Dragon1
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class Baron1
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class Blood1
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class Tower1
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class Inhibitor1
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class DragonSoul
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class AirSoul
    {
        public List<double> win { get; set; }
    }

    public class WaterSoul
    {
        public List<double> win { get; set; }
    }

    public class FireSoul
    {
        public List<double> win { get; set; }
    }

    public class EarthSoul
    {
        public List<double> win { get; set; }
        public List<double> lose { get; set; }
    }

    public class Objective
    {
        public Blue blue { get; set; }
        public Rift1 rift1 { get; set; }
        public Rift2 rift2 { get; set; }
        public RiftDouble riftDouble { get; set; }
        public EliteDouble eliteDouble { get; set; }
        public Dragon1 dragon1 { get; set; }
        public Baron1 baron1 { get; set; }
        public Blood1 blood1 { get; set; }
        public Tower1 tower1 { get; set; }
        public Inhibitor1 inhibitor1 { get; set; }
        public DragonSoul dragonSoul { get; set; }
        public AirSoul airSoul { get; set; }
        public WaterSoul waterSoul { get; set; }
        public FireSoul fireSoul { get; set; }
        public EarthSoul earthSoul { get; set; }
    }

    public class ItemSet1
    {
        public List<int> _6630 { get; set; }
    }

    public class ItemSet2
    {
        public List<int> _6630_6333 { get; set; }
        public List<int> _6630_3065 { get; set; }
    }

    public class ItemBootSet1
    {
        public List<int> _3047 { get; set; }
    }

    public class ItemBootSet2
    {
        public List<int> _3047_6630 { get; set; }
        public List<int> _6630_3111 { get; set; }
        public List<int> _3111_6630 { get; set; }
        public List<int> _6630_3047 { get; set; }
    }

    public class ItemBootSet3
    {
        public List<int> _3047_6630_3065 { get; set; }
    }

    public class ItemSets
    {
        public ItemSet1 itemSet1 { get; set; }
        public ItemSet2 itemSet2 { get; set; }
        public ItemBootSet1 itemBootSet1 { get; set; }
        public ItemBootSet2 itemBootSet2 { get; set; }
        public ItemBootSet3 itemBootSet3 { get; set; }
    }

    public class Response
    {
        public string platform { get; set; }
        public int version { get; set; }
        public string endPoint { get; set; }
        public bool valid { get; set; }
        public string duration { get; set; }
    }

    public class Root
    {
        public Header header { get; set; }
        public Graph graph { get; set; }
        public Nav nav { get; set; }
        public int analysed { get; set; }
        public double avgWinRate { get; set; }
        public List<List<object>> top { get; set; }
        public List<object> depth { get; set; }
        public int n { get; set; }

        //public Skills skills { get; set; }
        //public Time time { get; set; }

        //public TimeWin timeWin { get; set; }
        //public TopStats topStats { get; set; }
        //public List<List<object>> stats { get; set; }
        //public int statsCount { get; set; }
        //public Runes runes { get; set; }
        //public Objective objective { get; set; }
        //public List<List<double>> spell { get; set; }
        //public List<List<object>> spells { get; set; }
        //public ItemSets itemSets { get; set; }
        //public List<List<double>> startItem { get; set; }
        //public List<List<object>> startSet { get; set; }
        //public List<List<double>> earlyItem { get; set; }
        //public List<List<double>> boots { get; set; }
        //public List<List<double>> mythicItem { get; set; }
        //public List<List<double>> popularItem { get; set; }
        //public List<List<double>> winningItem { get; set; }
        //public List<List<double>> item { get; set; }
        //public List<List<double>> item1 { get; set; }
        //public List<List<double>> item2 { get; set; }
        public List<List<double>> enemy_top { get; set; }

        public List<List<double>> enemy_jungle { get; set; }
        public List<List<double>> enemy_middle { get; set; }
        public List<List<double>> enemy_bottom { get; set; }
        public List<List<double>> enemy_support { get; set; }

        //public Response response { get; set; }
        //public List<object> earlySet { get; set; }
        public List<List<double>> team_top { get; set; }

        public List<List<double>> team_jungle { get; set; }
        public List<List<double>> team_middle { get; set; }
        public List<List<double>> team_bottom { get; set; }
        public List<List<double>> team_support { get; set; }
    }
}