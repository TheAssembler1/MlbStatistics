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
        public string _letter { get; set; }
        public string _uriPrefix { get; set; }

        public PlayerNameUri(string name, string uri, string letter, string uriPrefix)
        {
            _name = name;
            _uri = uri;
            _letter = letter;
            _uriPrefix = uriPrefix;
        }
    }
}
