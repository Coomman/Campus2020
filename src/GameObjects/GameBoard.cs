using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thegame.GameObjects
{
    public class GameBoard
    {
        private byte[][] _board;

        public GameBoard(int width, int height)
        {
            _board = new byte[width][];
            for (int i = 0; i < width; i++)
            {
                _board[i] = new byte[height];
            }
        }


    }
}
