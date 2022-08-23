using HtmlAgilityPack;
using System.Net;

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

        private static List<HtmlNode> ParseHtml(string htmlText, string id, string selector)
        {
            List<HtmlNode> list = new();

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlText);

            var currentPlayerNodes = htmlDoc.GetElementbyId(id).SelectNodes(selector);

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

        public static async Task<List<HtmlNode>> GetResultFromUri(string routePrefix, string selector)
        {
            List<HtmlNode> resultNodes = new List<HtmlNode>();

            //checking if user has request /players url
            if (routePrefix.StartsWith("/players/"))
            {
                Console.WriteLine("successful in finding route for route prefix");

                string route = TEMP_ROUTE + routePrefix;
                string id = "div_players_";

                Console.WriteLine($"final route {route}");

                string htmlText = await GetHtmlStringAsync(route);
                resultNodes = ParseHtml(htmlText, id, selector);

                Console.WriteLine($"route: {route}");

                if (resultNodes == null)
                {
                    Console.WriteLine("failed to instantiate result of route prefix");
                    return new List<HtmlNode>();
                }
                else
                    Console.WriteLine("successfully instantiated result of route previx");
            }
            else
            {
                Console.WriteLine("failed to find route for route prefix");
            }

            return resultNodes;
        }

        public static async Task<List<List<string>>> GetAllPlayers()
        {
            List<List<string>> result = new List<List<string>>();
            string letters = "abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < letters.Length; i++)
            {
                string routePrefix = "/players/" + letters[i];
                List<HtmlNode> list = await WebScraper.GetResultFromUri(routePrefix, "//p/b/a");
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
                List<HtmlNode> list = await WebScraper.GetResultFromUri(routePrefix, "//p/b/a");
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

            for(int i = 0; i < letters.Length; i++)
            {
                string routePrefix = "/players/" + letters[i];
                List<HtmlNode> list = await WebScraper.GetResultFromUri(routePrefix, "//p/b/a");
                List<PlayerNameUri> currentListPlayerNameUri = new List<PlayerNameUri>();

                foreach (var node in list)
                    currentListPlayerNameUri.Add(new PlayerNameUri(node.InnerHtml, node.Attributes["href"].Value));

                result.Add(currentListPlayerNameUri);
            }

            return result;
        }

        public static async Task<List<PlayerNameUri>> GetAllPlayersWithLetterNameUri(char letter)
        {
            List<PlayerNameUri> result = new List<PlayerNameUri>();

            string routePrefix = "/players/" + letter;

            Console.WriteLine(routePrefix);
            List<HtmlNode> list = await WebScraper.GetResultFromUri(routePrefix, "//p/b/a");

            foreach (var node in list)
                result.Add(new PlayerNameUri(node.InnerHtml, node.Attributes["href"].Value));

            return result;
        }

        public static async Task<List<string>> GetAllPlayersWithLetter(char letter)
        {
            List<string> result = new List<string>();

            string routePrefix = "/players/" + letter;

            Console.WriteLine(routePrefix);
            List<HtmlNode> list = await WebScraper.GetResultFromUri(routePrefix, "//p//b/a");

            foreach (var node in list)
                result.Add(node.InnerHtml);

            return result;
        }
    }
}