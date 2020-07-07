using System;
using System.Collections.Generic;
using System.Linq;

namespace thegame.GameObjects
{
    public class GameCoordinator
    {
        private GameBoard _gameBoard;

        private readonly Dictionary<char, Direction> _keyBinds = new Dictionary<char, Direction>
        {
            [(char)87] = Direction.Up,
            [(char)83] = Direction.Down,
            [(char)65] = Direction.Left,
            [(char)68] = Direction.Right
        };

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

        public void GameTick(char key)
        {
            var dir = _keyBinds[key];

            MakeMove(dir);

            _gameBoard.CreateRandomGameCell();
        }

        public GameBoard StartGame(int width, int height)
        {
            _gameBoard = new GameBoard(width, height);

            _gameBoard.CreateRandomGameCell();
            _gameBoard.CreateRandomGameCell();

            return _gameBoard;
        }
    }
}
