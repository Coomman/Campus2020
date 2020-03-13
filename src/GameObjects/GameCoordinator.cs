using System;
using System.Collections.Generic;
using System.Linq;

namespace thegame.GameObjects
{
    public class GameCoordinator
    {
        private GameBoard _gameBoard;

        private readonly bool[] _possibleMoves;
        private bool _gameOver;

        private readonly Dictionary<ConsoleKey, Direction> _keyBinds = new Dictionary<ConsoleKey, Direction>
        {
            [ConsoleKey.UpArrow] = Direction.Up,
            [ConsoleKey.DownArrow] = Direction.Down,
            [ConsoleKey.LeftArrow] = Direction.Left,
            [ConsoleKey.RightArrow] = Direction.Right
        };

        public GameCoordinator()
        {
            _possibleMoves = Enumerable.Repeat(true, _possibleMoves.Length).ToArray();
        }

        private void MakeMove(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    _gameBoard.MoveUp();
                    return;
                case Direction.Down:
                    _gameBoard.MoveDown();
                    return;
                case Direction.Left:
                    _gameBoard.MoveLeft();
                    return;
                case Direction.Right:
                    _gameBoard.MoveRight();
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
        }

        private void GameTick()
        {
            var dir = _keyBinds[Console.ReadKey().Key];
            if (!_possibleMoves[(int) dir])
                return;

            MakeMove(dir);

            if(_gameBoard.GameOverCheck(_possibleMoves))
                _gameOver = true;

            //
        }

        public void StartGame(int width, int height)
        {
            _gameBoard = new GameBoard(width, height);

            while (!_gameOver)
            {
                GameTick();
            }
        }
    }
}
