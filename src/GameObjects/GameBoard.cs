using System;
using System.Collections.Generic;
using System.Linq;
using thegame.Models;

namespace thegame.GameObjects
{
    public class GameBoard
    {
        private readonly int[,] _board;

        public int Score { get; private set; }

        public int Width => _board.GetLength(0);
        public int Height => _board.GetLength(1);

        public GameBoard(int width, int height)
        {
            _board = new int[width, height];
        }

        public bool MoveUp(bool isFake = false)
        {
            throw new NotImplementedException();
        }
        public bool MoveLeft(bool isFake = false)
        {
            throw new NotImplementedException();
        }
        public bool MoveDown(bool isFake = false)
        {
            throw new NotImplementedException();
        }
        public bool MoveRight(bool isFake = false)
        {
            throw new NotImplementedException();
        }

        public bool GameOverCheck(bool[] possibleMoves)
        {
            possibleMoves[(int) Direction.Up] = MoveUp(true);
            possibleMoves[(int) Direction.Down] = MoveDown(true);
            possibleMoves[(int) Direction.Left] = MoveLeft(true);
            possibleMoves[(int) Direction.Right] = MoveRight(true);

            return possibleMoves.Contains(true);
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

        public CellDto[] ToDto()
        {
            var board = new List<CellDto>(Width * Height);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    board.Add(new CellDto($"{i},{j}",
                        new Vec(i, j),
                        _board[i, j].ToString(),
                        ((int)Math.Pow(2, _board[i, j])).ToString(), 
                        0));
                }
            }

            return board.ToArray();
        }
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}