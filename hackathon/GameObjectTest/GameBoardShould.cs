using System.Linq;
using NUnit.Framework;
using thegame.GameObjects;
using FluentAssertions;

namespace GameBoardTests
{
    [TestFixture]
    public class GameBoard_should
    {
        public GameBoard GameBoard;

        [SetUp]
        public void SetUp()
        {
            GameBoard = new GameBoard(5, 5);
        }

        [TearDown]
        public void TearDown()
        {
            GameBoard = null;
        }

        [Test]
        public void CorrectCreateRandomCell()
        {
            GameBoard.CreateRandomGameCell();
            var count = GameBoard.Board.Cast<int>().Sum(item => item == 0 ? 0 : 1);

            count.Should().Be(1);
        }
        [Test]
        public void CorrectCreateMoreThanOneCell()
        {
            GameBoard.CreateRandomGameCell();
            GameBoard.CreateRandomGameCell();
            var count = GameBoard.Board.Cast<int>().Sum(item => item == 0 ? 0 : 1);

            count.Should().Be(2);
        }
        
        [Test]
        public void CorrectShiftLine()
        {
            var line = new int[] {1, 1, 1, 2, 0};
            GameBoard.TryMoveArray(line, false, out var shiftedLine);

            shiftedLine.Should().BeEquivalentTo(new int[]{-1, -1, -1, -1, -1});
        }
    }
}