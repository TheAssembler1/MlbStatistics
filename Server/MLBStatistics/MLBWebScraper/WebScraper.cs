using HtmlAgilityPack;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

//FIXME::as of now this only gets currently players not old players

namespace MLBWebScraper
{
    public class WebScraper
    {
        private static readonly HttpClient httpClient = new();
        private static readonly string TEMP_ROUTE = "https://www.baseball-reference.com";

        public static List<string> stats = new List<string>
            {
                "year_ID",
                "age",
                "G",
                "PA",
                "AB",
                "R",
                "H",
                "2B",
                "3B",
                "HR",
                "RBI",
                "SB",
                "CS",
                "BB",
                "SO",
                "batting_avg",
                "onbase_perc",
                "slugging_perc",
                "onbase_plus_slugging",
                "onbase_plus_slugging_plus",
                "TB",
                "GIDP",
                "HBP",
                "SH",
                "SF",
                "IBB",
                "pos_season",
                "award_summar"
            };

        private static async Task<String> GetHtmlStringAsync(string uri)
        {
            string htmlText = string.Empty;

            try
            {
                Console.WriteLine("sucessfully obtained html text");
                htmlText = await httpClient.GetStringAsync(uri);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("failed to obtain html text");
                Console.WriteLine($"{e.Message}");
            }

            return htmlText;
        }

        private static List<HtmlNode> ParseHtml(string htmlText, string id, string selector, Func<HtmlNode, bool> function)
        {
            List<HtmlNode> list = new();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlText);

            Console.WriteLine(selector);
            Console.WriteLine(id);

            var currentPlayerNodes = htmlDoc.GetElementbyId(id).SelectNodes(selector).Where(function);

            if (currentPlayerNodes == null)
            {
                Console.WriteLine("failed to find nodes with given selector");
            }
            else
            {
                Console.WriteLine("successfully found nodes with given selector");
                foreach (var node in currentPlayerNodes)
                    list.Add(node);
            }

            return list;
        }

        public static async Task<List<HtmlNode>> GetResultFromUri(string id, string routePrefix, string selector, Func<HtmlNode, bool> function)
        {
            List<HtmlNode> resultNodes = new List<HtmlNode>();

            Console.WriteLine("successful in finding route for route prefix");

            string route = TEMP_ROUTE + routePrefix;

            Console.WriteLine($"final route {route}");

            string htmlText = await GetHtmlStringAsync(route);
            resultNodes = ParseHtml(htmlText, id, selector, function);

            Console.WriteLine($"route: {route}");

            Console.WriteLine($"Total nodes found: {resultNodes.Count}");

            if (resultNodes == null)
            {
                Console.WriteLine("failed to instantiate result of route prefix");
                return new List<HtmlNode>();
            }
            else
                Console.WriteLine("successfully instantiated result of route previx");

            return resultNodes;
        }

        public static async Task<List<List<string>>> GetAllPlayers()
        {
            List<List<string>> result = new List<List<string>>();
            string letters = "abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < letters.Length; i++)
            {
                string routePrefix = "/players/" + letters[i];
                List<HtmlNode> list = await WebScraper.GetResultFromUri("div_players_", routePrefix, "//p/b/a", (node) => true);
                List<string> currentStringList = new List<string>();

                foreach (var node in list)
                    currentStringList.Add(node.InnerHtml);

                result.Add(currentStringList);
            }

            return result;
        }

        public static async Task<List<List<string>>> GetAllPlayersUris()
        {
            List<List<string>> result = new List<List<string>>();
            string letters = "abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < letters.Length; i++)
            {
                string routePrefix = "/players/" + letters[i];

                //need a parameter to filter through the html code here
                Console.WriteLine(routePrefix);
                List<HtmlNode> list = await WebScraper.GetResultFromUri("div_players_", routePrefix, "//p/b/a", (node) => true);
                List<string> currentListString = new List<string>();

                foreach (var node in list)
                    currentListString.Add(node.Attributes["href"].Value);

                result.Add(currentListString);
            }

            return result;
        }

        public static async Task<List<List<PlayerNameUri>>> GetAllPlayersNameUri()
        {
            List<List<PlayerNameUri>> result = new List<List<PlayerNameUri>>();
            string letters = "abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < letters.Length; i++)
            {
                string routePrefix = "/players/" + letters[i];
                List<HtmlNode> list = await WebScraper.GetResultFromUri("div_players_", routePrefix, "//p/b/a", (node) => true);
                List<PlayerNameUri> currentListPlayerNameUri = new List<PlayerNameUri>();

                foreach (var node in list)
                {
                    const int letterPosition = 1;
                    const int playerUriPosition = 2;

                    string uri = node.Attributes["href"].Value;
                    string[] uriSplit = uri.Split("/");

                    currentListPlayerNameUri.Add(new PlayerNameUri(node.InnerHtml, uri, uriSplit[letterPosition], uriSplit[playerUriPosition]));
                }

                result.Add(currentListPlayerNameUri);
            }

            return result;
        }

        public static async Task<List<PlayerNameUri>> GetAllPlayersWithLetterNameUri(char letter)
        {
            List<PlayerNameUri> result = new List<PlayerNameUri>();

            string routePrefix = "/players/" + letter;

            Console.WriteLine(routePrefix);
            List<HtmlNode> list = await WebScraper.GetResultFromUri("div_players_", routePrefix, "//p//b/a", (node) => true);

            foreach (var node in list)
            {
                const int letterPosition = 1;
                const int playerUriPosition = 2;

                string uri = node.Attributes["href"].Value;
                string[] uriSplit = uri.Split("/");

                //adding the total uri and the split should prolly put these in const
                result.Add(new PlayerNameUri(node.InnerHtml, uri, uriSplit[letterPosition], uriSplit[playerUriPosition]));
            }

            return result;
        }


        private static Func<HtmlNode, bool> testMinorTable = (HtmlNode node) =>
        {
            string result = node.ParentNode.GetAttributeValue("class", null);
            return result != "minors_table hidden";
        };

        public static async Task<List<string>> GetAllPlayersWithUriStat(string letter, string uri, string stat)
        {
            List<string> result = new List<string>();
            string routePrefix = $"/players/{letter}/{uri}";
            Console.WriteLine(routePrefix);
            string selector = (stat == "year_ID") ? "//tbody//tr//th" : "//tbody//tr//td";
            List<HtmlNode> list = await WebScraper.GetResultFromUri("batting_standard", routePrefix, selector, testMinorTable);

            switch (stat)
            {
                case "year_ID":
                    list.ForEach((node) =>
                    {
                        string temp = node.GetAttributeValue("csk", null);

                        if (temp != null)
                            result.Add(temp);
                    });
                    break;
                case "age":
                case "G":
                case "PA":
                case "AB":
                case "R":
                case "H":
                case "2B":
                case "3B":
                case "HR":
                case "RBI":
                case "SB":
                case "CS":
                case "BB":
                case "SO":
                case "batting_avg":
                case "onbase_perc":
                case "slugging_perc":
                case "onbase_plus_slugging":
                case "onbase_plug_slugging_plus":
                case "TB":
                case "GIDP":
                case "HBP":
                case "SH":
                case "SF":
                case "IBB":
                case "pos_season":
                case "award_summary":
                    list.ForEach((node) =>
                    {
                        bool nodeFound = node.GetAttributeValue("data-stat", null) == stat;

                        if(nodeFound)
                            result.Add(node.InnerHtml);
                    });
                    break;
                default:
                    Console.WriteLine("stat does not exist or is not implemented");
                    break;
            }

            return result;
        }

        public static async Task<List<string>> GetAllPlayersWithLetter(char letter)
        {
            List<string> result = new List<string>();

            string routePrefix = "/players/" + letter;

            Console.WriteLine(routePrefix);
            List<HtmlNode> list = await WebScraper.GetResultFromUri("div_players_", routePrefix, "//p//b/a", (node) => true);

            foreach (var node in list)
                result.Add(node.InnerHtml);

            return result;
        }
    }
}