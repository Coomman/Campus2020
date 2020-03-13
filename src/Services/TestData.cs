using System;
using thegame.GameObjects;
using thegame.Models;

namespace thegame.Services
{
    public class TestData
    {
        public static GameDto AGameDto(GameBoard gameBoard)
        {
            return gameBoard.ToDto();
        }
    }
}