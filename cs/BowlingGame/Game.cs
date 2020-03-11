using System;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        private int score = 0; 

        public void Roll(int pins)
        {
            score += pins;
        }

        public int GetScore()
        {
            return score;
        }
    }

    [TestFixture]
    public class Game_Should : ReportingTest<Game_Should>
    {
        private Game game;

        [SetUp]
        public void Setup()
        {
            game = new Game();
        }

        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            game.GetScore().Should().Be(0);
        }

        [Test]
        public void HavePinsScore_AfterFirstRoll()
        {
            game.Roll(7);
            game.GetScore().Should().Be(7);
        }

        [Test]
        public void HaveBothPinsScore_AfterOneFrame()
        {
            game.Roll(6);
            game.Roll(3);
            game.GetScore().Should().Be(9);
        }
    }
}
