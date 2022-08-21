using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using MLBWebScraper;

namespace Players.Controllers
{
    [Route("[controller]/")]
    [EnableCors("React")]
    [ApiController]
    public class Players : ControllerBase
    {
        [HttpGet("Uris")]
        public async Task<List<List<string>>> GetAllPlayersUris()
        {
            List<List<string>> list = await WebScraper.GetAllPlayersUris();

            foreach(var item in list)
            {
                foreach(var playerUri in item)
                    Console.WriteLine(playerUri);
            }

            //FIXME::not sure if this works
            if (list.Count <= 0)
                NotFound();

            return list;
        }


        [HttpGet("Names")]
        public async Task<List<List<string>>> GetAllPlayers()
        {
            List<List<string>> list = await WebScraper.GetAllPlayers();

            foreach (var item in list)
            {
                foreach (var player in item)
                    Console.WriteLine(player);
            }
                
            //FIXME::not sure if this works
            if (list.Count <= 0)
                NotFound();

            return list;
        }


        //TODO::implement this to fetch all the players with a given letter.
        [HttpGet("{letter}")]
        public async Task<List<string>> GetAllPlayersWithLetter(char letter)
        {
            if (letter > 'z' || letter < 'a')
                return await Task.FromResult(new List<string>());

            List<string> list = await WebScraper.GetAllPlayersWithLetter(letter);
            return list;
        }

        [HttpGet("NamesUris")]
        public async Task<List<string>> GetAllPlayersNameUri()
        {
            List<List<PlayerNameUri>> list = await WebScraper.GetAllPlayersNameUri();
            List<string> result = new List<string>();

            foreach (var item in list) {
               foreach(var playerNameUri in item)
                {
                    result.Add(playerNameUri._name);
                    result.Add(playerNameUri._uri);
                }
            }

            return result;
        }
    }
}
