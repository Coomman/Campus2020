using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thegame.GameObjects
{
    public class GameBoard
    {
        private int[,] _board;

        public GameBoard(int width, int height)
        {
            _board = new int[width, height];
        }

        public bool[] TryAllMoves()
        {
            throw new NotImplementedException();
        }
        public bool TryMoveUp()
        {
            throw new NotImplementedException();
        }
        public bool TryMoveLeft()
        {
            throw new NotImplementedException();
        }
        public bool TryMoveDown()
        {
            throw new NotImplementedException();
        }
        public bool TryMoveRight()
        {
            throw new NotImplementedException();
        }
        public bool GameOverCheck()
        {
            throw new NotImplementedException();
        }
        public void CreateGameCell()
        {
            var availableCells = new List<Tuple<int, int>>();
            for (var i = 0; i < _board.GetLength(0); i++)
            for (var j = 0; j < _board.GetLength(1); j++)
                if (_board[i, j] == 0)
                    availableCells.Add(Tuple.Create(i, j));

            var rnd = new Random();
            var (width, height) = availableCells[rnd.Next(availableCells.Count)];
            _board[width, height] = rnd.Next(4) < 0 ? 1 : 2;
        }
    }
}