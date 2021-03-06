using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using thegame.Models;
using thegame.Services;

using static thegame.Services.GamesRepo;

namespace thegame.Controllers
{
    [Route("api/games/{gameId}/moves")]
    public class MovesController : Controller
    {
        [HttpPost]
        public IActionResult Moves(Guid gameId, [FromBody]UserInputForMovesPost userInput)
        {
            var key = (int)userInput.KeyPressed;
            if (key == 65 || key == 68 || key == 83 || key == 87)
                Op.GameTick(userInput.KeyPressed);

            return new ObjectResult(Board.ToDto());
        }
    }
}