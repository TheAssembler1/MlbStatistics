using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using MLBWebScraper;

namespace AllPlayers.Controllers
{
    [Route("[controller]/AllPlayers")]
    [EnableCors("React")]
    [ApiController]
    public class AllPlayers : ControllerBase
    {
        [HttpGet(Name = "GetAllPlayers")]
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
    }
}
