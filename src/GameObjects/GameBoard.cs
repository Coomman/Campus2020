using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using thegame.Models;

namespace thegame.GameObjects
{
    public class GameBoard
    {
        public int[,] Board { get; }

        public int Score { get; private set; }

        public int Width => Board.GetLength(0);
        public int Height => Board.GetLength(1);

        public GameBoard(int width, int height)
        {
            Board = new int[width, height];
        }

        #region moves
        public bool ManageLine(int[] line, bool isFake)
        {
            var clone = (int[]) line.Clone();

            var q = new Queue<int>();
            foreach (int n in line)
                if (n != 0)
                    q.Enqueue(n);

            for (int i = 0; i < line.Length; i++)
            {
                line[i] = 0;
            }

            if (q.Count == 0)
                return false;

            int index = 0;
            int cur = line[index] = q.Dequeue();

            while(q.Count != 0)
            {
                var next = q.Dequeue();
                if (cur == next)
                {
                    line[index++] = cur + 1;
                    
                    if(!isFake)
                        Score += (int) Math.Pow(2, line[index - 1]);

                    if (q.Count == 0)
                        break;

                    cur = line[index] = q.Dequeue();
                    continue;
                }

                line[index++] = cur;
                cur = line[index] = next;
            }

            return line.SequenceEqual(clone);
        }

        public bool MoveUp(bool isFake = false)
        {
            var locked = false;

            for (int i = 0; i < Width; i++)
            {
                var arr = new int[Height];
                for (int j = 0; j < Height; j++)
                    arr[j] = Board[i, j];

                locked |= ManageLine(arr, isFake);

                if (isFake)
                    continue;

                for (int j = 0; j < Height; j++)
                    Board[i, j] = arr[j];
            }

            return locked;
        }
        public bool MoveLeft(bool isFake = false)
        {
            var locked = false;

            for (int i = 0; i < Height; i++)
            {
                var arr = new int[Width];
                for (int j = 0; j < Width; j++)
                    arr[j] = Board[j, i];

                locked |= ManageLine(arr, isFake);

                if (isFake)
                    continue;

                for (int j = 0; j < Width; j++)
                    Board[j, i] = arr[j];
            }

            return locked;
        }
        public bool MoveRight(bool isFake = false)
        {
            var locked = false;

            for (int i = 0; i < Height; i++)
            {
                var arr = new int[Width];
                for (int j = 0; j < Width; j++)
                    arr[j] = Board[Width - j - 1, i];

                locked |= ManageLine(arr, isFake);

                if (isFake)
                    continue;

                for (int j = 0; j < Width; j++)
                    Board[Width - j - 1, i] = arr[j];

            }

            return locked;
        }
        public bool MoveDown(bool isFake = false)
        {
            var locked = false;

            for (int i = 0; i < Width; i++)
            {
                var arr = new int[Height];
                for (int j = 0; j < Height; j++)
                    arr[j] = Board[i, Height - j - 1];

                locked |= ManageLine(arr, isFake);

                if (isFake)
                    continue;

                for (int j = 0; j < Height; j++)
                    Board[i, j] = arr[Height - j - 1];
            }

            return locked;
        }

        #endregion

        public bool GameOverCheck()
        {
            return !MoveUp(true) && !MoveDown(true) && !MoveLeft(true) && !MoveRight(true);
        }

        public void CreateRandomGameCell()
        {
            var availableCells = new List<Tuple<int, int>>();
            for (var i = 0; i < Board.GetLength(0); i++)
            for (var j = 0; j < Board.GetLength(1); j++)
                if (Board[i, j] == 0)
                    availableCells.Add(Tuple.Create(i, j));

            var rnd = new Random();
            var (width, height) = availableCells[rnd.Next(availableCells.Count - 1)];
            Board[width, height] = rnd.Next(4) != 0 ? 1 : 2;
        }

        public GameDto ToDto()
        {
            var board = new List<CellDto>(Width * Height);
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var num = ((int) Math.Pow(2, Board[i, j])).ToString();

                    board.Add(new CellDto($"{i},{j}",
                        new Vec(i, j),
                        Board[i, j] == 0 ? "field": $"tile-{num}",
                        Board[i,j] == 0 ? "" : num, 
                        0));
                }
            }

            return new GameDto(board.ToArray(),
                true, false,
                Width, Height,
                Guid.Empty, GameOverCheck(), Score);
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