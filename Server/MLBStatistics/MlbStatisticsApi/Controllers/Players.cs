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
            return list;
        }


        [HttpGet("Names")]
        public async Task<List<List<string>>> GetAllPlayers()
        {
            List<List<string>> list = await WebScraper.GetAllPlayers();
            return list;
        }

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
