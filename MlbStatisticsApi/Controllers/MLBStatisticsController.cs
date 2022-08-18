using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MLBWebScraper;

namespace MlbStatisticsApi.Controllers
{
    [EnableCors("React")]
    [ApiController]
    [Route("[controller]/GetAllPlayers")]
    public class MlbStatisticsController : ControllerBase
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
