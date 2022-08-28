using MLBWebScraper;

namespace MLBStatistics
{
    class MLBStatistics
    {
        public static async Task Main()
        {
            string playerUri = "troutmi01.shtml";
            string letter = "t";

            List<string> stats = WebScraper.stats;

            foreach(string stat in stats)
            {
                Console.WriteLine("___________________________________________________________________");
                Console.WriteLine(stat);
                List<string> temp = await WebScraper.GetAllPlayersWithUriStat(letter, playerUri, stat);
                Console.WriteLine($"Length of list is: {temp.Count}");
                temp.ForEach((item) => Console.WriteLine(item));
                Console.WriteLine("___________________________________________________________________");
            }
        }
    }
}