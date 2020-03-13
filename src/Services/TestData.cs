using System;
using thegame.GameObjects;
using thegame.Models;

namespace thegame.Services
{
    public class TestData
    {
        public static GameDto AGameDto(GameBoard gameBoard)
        {
            return new GameDto(gameBoard.ToDto(),
                true, false, 
                gameBoard.Width,gameBoard.Height,
                Guid.Empty, false, gameBoard.Score);
        }
    }
}