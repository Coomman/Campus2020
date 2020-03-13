using System;
using System.Collections.Generic;
using System.Linq;
using thegame.Models;

namespace thegame.GameObjects
{
    public class GameBoard
    {
        public int[,] Board { private set; get; }

        public int Score { get; private set; }

        public int Width => Board.GetLength(0);
        public int Height => Board.GetLength(1);

        public GameBoard(int width, int height)
        {
            Board = new int[width, height];
        }

        #region moves

        //public static bool TryMoveArray(int[] row, bool isFake, out int[] shiftRow)
        //{
        //    shiftRow = (int[]) row.Clone();
        //    for (var i = row.Length - 1; i >= 0; i--)
        //    {
        //        if (row[i] == 0 && isFake)
        //            return true;
        //        for (var j = i; j < row.Length - 1; j++)
        //        {
        //            if (row[j] != row[j + 1]) continue;
        //            row[j + 1] += 1;
        //            shiftRow[j] = 0;
        //            if (isFake)
        //                return true;
        //        }
        //    }

        //    return !isFake;
        //}


        //public bool MoveUp(bool isFake = false)
        //{
        //    var localBoard = (int[,]) Board.Clone();
        //    for (var i = 1; i < localBoard.GetLength(0); i++)
        //    {
        //        var row = new int[localBoard.GetLength(1)];
        //        for (var j = 0; j < localBoard.GetLength(1); j++)
        //            row[j] = localBoard[j, i];

        //        if (!TryMoveArray(row, isFake, out var shiftedRow)) continue;
        //        if (isFake)
        //            return true;
        //        for (var j = 0; j < localBoard.GetLength(1); j++)
        //            localBoard[j, i] = row[j];
        //    }

        //    if (isFake)
        //        return false;
        //    Board = (int[,]) localBoard.Clone();
        //    return true;
        //}

        //public bool MoveLeft(bool isFake = false)
        //{
        //    var localBoard = (int[,]) Board.Clone();
        //    for (var i = 1; i < localBoard.GetLength(1); i++)
        //    {
        //        var row = new int[localBoard.GetLength(0)];
        //        for (var j = 0; j < localBoard.GetLength(0); j++)
        //            row[j] = localBoard[i, j];

        //        if (!TryMoveArray(row, isFake, out var shiftedRow)) continue;
        //        if (isFake)
        //            return true;
        //        for (var j = 0; j < localBoard.GetLength(0); j++)
        //            localBoard[i, j] = row[j];
        //    }

        //    if (isFake)
        //        return false;
        //    Board = (int[,]) localBoard.Clone();
        //    return true;
        //}

        //public bool MoveDown(bool isFake = false)
        //{
        //    var localBoard = (int[,]) Board.Clone();
        //    for (var i = 1; i < localBoard.GetLength(0); i++)
        //    {
        //        var row = new int[localBoard.GetLength(1)];
        //        for (var j = 0; j < localBoard.GetLength(1); j++)
        //            row[j] = localBoard[localBoard.GetLength(1) - j - 1, i];

        //        if (!TryMoveArray(row, isFake, out var shiftedRow)) continue;
        //        if (isFake)
        //            return true;
        //        for (var j = 0; j < localBoard.GetLength(1); j++)
        //            localBoard[localBoard.GetLength(1) - j - 1, i] = row[j];
        //    }

        //    if (isFake)
        //        return false;
        //    Board = (int[,]) localBoard.Clone();
        //    return true;
        //}

        //public bool MoveRight(bool isFake = false)
        //{
        //    var localBoard = (int[,]) Board.Clone();
        //    for (var i = 1; i < localBoard.GetLength(1); i++)
        //    {
        //        var row = new int[localBoard.GetLength(0)];
        //        for (var j = 0; j < localBoard.GetLength(0); j++)
        //            row[j] = localBoard[i, localBoard.GetLength(0) - j - 1];

        //        if (!TryMoveArray(row, isFake, out var shiftedRow)) continue;
        //        if (isFake)
        //            return true;
        //        for (var j = 0; j < localBoard.GetLength(0); j++)
        //            localBoard[i, localBoard.GetLength(0) - j - 1] = row[j];
        //    }

        //    if (isFake)
        //        return false;
        //    Board = (int[,]) localBoard.Clone();
        //    return true;
        //}

        public bool MoveUp(bool isFake = false)
        {
            Board = new int[Width, Height];
            return true;
        }
        public bool MoveRight(bool isFake = false)
        {
            Board = new int[Width, Height];
            return true;
        }
        public bool MoveLeft(bool isFake = false)
        {
            Board = new int[Width, Height];
            return true;
        }
        public bool MoveDown(bool isFake = false)
        {
            Board = new int[Width, Height];
            return false;
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