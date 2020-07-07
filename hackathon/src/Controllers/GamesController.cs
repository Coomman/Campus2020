using Microsoft.AspNetCore.Mvc;
using thegame.GameObjects;
using thegame.Models;
using thegame.Services;
using static thegame.Services.GamesRepo;

namespace thegame.Controllers
{
    [Route("api/games")]
    public class GamesController : Controller
    {
        [HttpPost]
        public IActionResult Index()
        {
            Op = new GameCoordinator();
            Board = Op.StartGame(4, 4);

            return new ObjectResult(TestData.AGameDto(Board));
        }
    }
}
