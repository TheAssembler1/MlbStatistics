using MLBWebScraper;

namespace MLBStatistics
{
    class MLBStatistics
    {
        public static async Task Main()
        {
            List<string> result = await WebScraper.GetAllPlayersWithUriYears("/players/t/troutmi01.shtml");
            result.ForEach((str) => Console.WriteLine(str));
        }
    }
}