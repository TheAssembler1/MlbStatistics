using MLBWebScraper;

namespace MLBStatistics
{
    class MLBStatistics
    {
        public static async Task Main()
        {
            List<string> years = await WebScraper.GetAllPlayersWithUriYears("t", "troutmi01.shtml");
            years.ForEach((str) => Console.WriteLine(str));

            List<string> ages = await WebScraper.GetAllPlayersWithUriStat("t", "troutmi01.shtml", "age");
            ages.ForEach((str) => Console.WriteLine(str));

            List<string> ba = await WebScraper.GetAllPlayersWithUriStat("t", "troutmi01.shtml", "batting_avg");
            ba.ForEach((str) => Console.WriteLine(str));

            Console.WriteLine(years.Count);
            Console.WriteLine(ages.Count);
            Console.WriteLine(ba.Count);
        }
    }
}