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
            catch(HttpRequestException e)
            {
                Console.WriteLine("failed to obtain html text");
                Console.WriteLine($"{e.Message}");
            }

            return htmlText;
        }

        private static List<string> ParseHtml(string htmlText, string id, string selector)
        {
            List<string> list = new();

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
                    list.Add(node.InnerHtml);
            }

            return list;
        }

        public static async Task<List<string>> GetResultFromUri(string routePrefix)
        {
            List<string> result = new List<string>();

            //checking if user has request /players url
            if (routePrefix.StartsWith("/players/"))
            {
                Console.WriteLine("successful in finding route for route prefix");

                string route = TEMP_ROUTE + routePrefix;
                string id = "div_players_";
                string selector = "//p/b/a";

                Console.WriteLine($"final route {route}");

                string htmlText = await GetHtmlStringAsync(route);
                result = ParseHtml(htmlText, id, selector);

                Console.WriteLine($"route: {route}");

                if (result == null)
                {
                    Console.WriteLine("failed to instantiate result of route prefix");
                    return new List<string>();
                }
                else
                    Console.WriteLine("successfully instantiated result of route previx");
            }
            else
            {
                Console.WriteLine("failed to find route for route prefix");
            }

            return result;
        }

        public static async Task<List<List<string>>> GetAllPlayers()
        {
            List<List<string>> result = new List<List<string>>();
            string letters = "abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < letters.Length; i++)
            {
                string routePrefix = "/players/" + letters[i];
                List<String> list = await WebScraper.GetResultFromUri(routePrefix);
                result.Add(list);
            }

            return result;
        }
    }
}