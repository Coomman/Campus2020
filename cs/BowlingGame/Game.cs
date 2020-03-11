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
        private readonly List<int> throws = new List<int>();

        public bool Spare { get; private set; }
        public bool Strike { get; private set; }

        public int AdditionalPoints { get; private set; }

        public bool MakeThrow(int pins)
        {
            throws.Add(pins);

            if (throws.Count() == 1)
            {
                if (pins == 10)
                    Strike = true;

                return Strike;
            }

            if (throws[0] + pins == 10)
                Spare = true;

            return true;
        }

        public void AddPoints(int points)
        {
            AdditionalPoints += points;
        }

        public int GetScore()
            => throws.Sum() + AdditionalPoints;
    }

    public class Game
    {
        private readonly List<Frame> frames = new List<Frame>(10) {new Frame()};

        private readonly int[] additionalPointsList = new int[10];

        public void Roll(int pins)
        {
            for (int i = 0; i < 10; i++)
            {
                if(additionalPointsList[i] > 0)
                    frames[i].AddPoints(pins);
            }

            bool frameEnded = frames.Last().MakeThrow(pins);

            if (!frameEnded)
                return;

            if (frames.Last().Spare)
                additionalPointsList[frames.Count - 1] = 1;

            if (frames.Last().Strike)
                additionalPointsList[frames.Count - 1] = 2;

            frames.Add(new Frame());
        }

        public int GetScore()
        {
            return frames.Sum(f => f.GetScore());
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

        [Test]
        public void HaveAdditionalScore_AfterSpare()
        {
            game.Roll(6);
            game.Roll(4);
            game.Roll(5);

            game.GetScore().Should().Be(20);
        }

        [Test]
        public void HaveCorrectScore_AfterStrikeStrike()
        {
            game.Roll(10);
            game.Roll(10);

            game.GetScore().Should().Be(30);
        }

        [Test]
        public void HaveCorrectScore_AfterMultipleStrikes()
        {
            game.Roll(10);
            game.Roll(10);
            game.Roll(10);

            game.GetScore().Should().Be(60);
        }
    }
}
