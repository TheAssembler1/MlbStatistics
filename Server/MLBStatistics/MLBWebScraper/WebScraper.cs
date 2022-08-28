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

        //NOTE::This only gets they major years not minor
        public static async Task<List<string>> GetAllPlayersWithUriYears(string letter, string uri)
        {
            List<string> result = new List<string>();

            string routePrefix = $"/players/{letter}/{uri}";

            Console.WriteLine(routePrefix);

            //FIXME::need to find the correct query here
            List<HtmlNode> list = await WebScraper.GetResultFromUri("batting_standard", routePrefix, "//tbody//tr//th", testMinorTable);

            list.ForEach((node) =>
            {
                //FIXME::for some reason sometimes the years are floats
                //FIMXE::need to look into this and see what the floats represent
                //FIXME::possible a break in the season or something?
                string temp = node.GetAttributeValue("csk", null);

                if(temp != null)
                    result.Add(temp);
            });

            return result;
        }

        public static async Task<List<string>> GetAllPlayersWithUriStat(string letter, string uri, string stat)
        {
            List<string> result = new List<string>();

            string routePrefix = $"/players/{letter}/{uri}";
            Console.WriteLine(routePrefix);

            //FIXME::need to find the correct query here
            List<HtmlNode> list = await WebScraper.GetResultFromUri("batting_standard", routePrefix, "//tbody//tr//td", testMinorTable);

            list.ForEach((node) => {
                if (node.GetAttributeValue("data-stat", null) == stat)
                    result.Add(node.InnerHtml);
            });

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