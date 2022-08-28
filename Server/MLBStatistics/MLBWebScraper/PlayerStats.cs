using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MLBWebScraper
{
    public class PlayerStats
    {
        public Dictionary<string, List<string>> _Stats = new Dictionary<string, List<string>>();

        public PlayerStats(List<List<string>> Stats)
        {
            for(int i = 0; i < WebScraper.stats.Count; i++)
            {
                _Stats.Add(WebScraper.stats[i], Stats[i]);
            }
        }

        public string getJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
