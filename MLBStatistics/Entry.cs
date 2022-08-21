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

            Console.WriteLine("got all the players names.\n\n\n");

            List<List<string>> list2 = await WebScraper.GetAllPlayersUris();

            foreach(var item in list2)
            {
                foreach(var playerHref in item)
                    Console.WriteLine(playerHref);
            }
        }
    }
}