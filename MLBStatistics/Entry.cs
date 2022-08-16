using MLBWebScraper;

namespace MLBStatistics
{
    class MLBStatistics
    {
        public static async Task Main()
        {
            List<List<string>> list = await WebScraper.GetAllPlayers();

            foreach(var item in list)
            {
                foreach(var player in item)
                    Console.WriteLine(player);
            }
        }
    }
}