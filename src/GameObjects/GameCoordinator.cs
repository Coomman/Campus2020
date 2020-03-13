using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thegame.GameObjects
{
    public class GameCoordinator
    {
        private bool[] _possibleMoves;
        private bool gameOver;

        public GameCoordinator()
        {
            _possibleMoves = Enumerable.Repeat(true, _possibleMoves.Length).ToArray();
        }

        private void GameTick()
        {
            var key = Console.ReadKey();
        }

        public void StartGame()
        {
            while (!gameOver)
            {
                GameTick();
            }
        }
    }
}
