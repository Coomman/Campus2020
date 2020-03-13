using System;
using System.Collections.Generic;
using System.Linq;

namespace thegame.GameObjects
{
    public class GameCoordinator
    {
        private GameBoard _gameBoard;

        private readonly bool[] _possibleMoves;
        public bool GameOver { get; private set; }

        private readonly Dictionary<char, Direction> _keyBinds = new Dictionary<char, Direction>
        {
            [(char)38] = Direction.Up,
            [(char)40] = Direction.Down,
            [(char)37] = Direction.Left,
            [(char)39] = Direction.Right
        };

        public GameCoordinator()
        {
            _possibleMoves = Enumerable.Repeat(true, 4).ToArray();
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

        public void GameTick(char key)
        {
            var dir = _keyBinds[key];
            if (!_possibleMoves[(int) dir])
                return;

            MakeMove(dir);

            if(_gameBoard.GameOverCheck(_possibleMoves))
                GameOver = true;
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
