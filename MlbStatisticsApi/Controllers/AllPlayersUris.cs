using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using MLBWebScraper;

namespace AllPlayersUris.Controllers
{
    [Route("[controller]/AllPlayersUris")]
    [EnableCors("React")]
    [ApiController]
    public class AllPlayerUris : ControllerBase
    {
        [HttpGet(Name = "GetAllPlayersUris")]
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
    }
}
