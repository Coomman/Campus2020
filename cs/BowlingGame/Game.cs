using System;
using System.Collections.Generic;
using System.Linq;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Frame
    {
        public int Score { get; private set; }

        public bool IsSpare { get; private set; }
        public bool IsStrike { get; private set; }

        public Frame(int lastFrameScore = 0)
        {
            Score = lastFrameScore;
        }

        public void MakeThrow(int pins)
        {
            Score += pins;
        }
    }

    public class Game
    {
        private bool isSecondThrow;

        private readonly List<Frame> frames = new List<Frame>(10) {new Frame()};

        public void Roll(int pins)
        {
            frames.Last().MakeThrow(pins);

            if (isSecondThrow)
            {
                frames.Add(new Frame(frames.Last().Score));
            }

            isSecondThrow = !isSecondThrow;
        }

        public int GetScore()
        {
            return frames.Last().Score;
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

        [Test]
        public void HaveAllScore_WithMultipleFrames()
        {
            game.Roll(6);
            game.Roll(3);
            game.Roll(2);

            game.GetScore().Should().Be(11);
        }

        //[Test]
        //public void HaveAdditionalScore_AfterSpare()
        //{
        //    game.Roll();
        //}`
    }
}
