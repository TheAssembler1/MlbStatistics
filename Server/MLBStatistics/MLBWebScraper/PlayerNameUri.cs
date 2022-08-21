using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLBWebScraper
{
    public class PlayerNameUri 
    {
        public string _name { get; set; }
        public string _uri { get; set; }

        public PlayerNameUri(string name, string uri)
        {
            _name = name;
            _uri = uri;
        }
    }
}
