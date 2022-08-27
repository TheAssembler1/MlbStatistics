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

        //NOTE::the player name should be the url stored with the player use PlayerNameUri
        [HttpGet("Names/Years/{letter}/{playerUri}")]
        public async Task<List<string>> GetAllPlayersWithNameWithYear(string letter, string playerUri)
        {
            List<string> list = await WebScraper.GetAllPlayersWithUriYears(letter, playerUri);

            if (list.Count == 0)
                list.Add("Nothing in list");

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
        public async Task<List<List<PlayerNameUri>>> GetAllPlayersNameUri()
        {
            List<List<PlayerNameUri>> list = await WebScraper.GetAllPlayersNameUri();
            return list;
        }

        [HttpGet("NamesUris/{letter}")]
        public async Task<List<PlayerNameUri>> GetAllPlayersNameWithLetterNameUri(char letter)
        {
            List<PlayerNameUri> list = await WebScraper.GetAllPlayersWithLetterNameUri(letter);

            if (letter > 'z' || letter < 'a')
                return await Task.FromResult(new List<PlayerNameUri>());

            return list;
        }
    }
}
