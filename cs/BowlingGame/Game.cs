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
        public int BonusPoints { get; private set; }

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
        public void AddBonusPoints(int points)
        {
            BonusPoints += points;
        }
        public int GetScore()
            => throws.Sum() + BonusPoints;
    }

    public class Game
    {
        private readonly List<Frame> frames = new List<Frame>(10) {new Frame()};

        private readonly int[] additionalPointsList = new int[10];

        private bool bonusGameActive;
        private int bonusGameFrames;

        private void BonusAccrual(int pins)
        {
            for (int i = 0; i < 10; i++)
            {
                if (additionalPointsList[i] <= 0)
                    continue;

                frames[i].AddBonusPoints(pins);
                additionalPointsList[i]--;
            }
        }

        private void CheckLastThrow(ref int value)
        {
            if (frames.Last().Spare)
                value = 1;

            if (frames.Last().Strike)
                value = 2;
        }

        public void Roll(int pins)
        {
            if(bonusGameActive && bonusGameFrames == 0)
                throw new NotSupportedException("Game over");

            BonusAccrual(pins);

            if (bonusGameActive)
            {
                bonusGameFrames--;
                return;
            }

            if (!frames.Last().MakeThrow(pins))
                return;

            CheckLastThrow(ref additionalPointsList[frames.Count - 1]);

            if (frames.Count == 10)
            {
                bonusGameActive = true;
                CheckLastThrow(ref bonusGameFrames);
                return;
            }

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
        public void GetScore_BeforeAnyRolls()
        {
            game.GetScore().Should().Be(0);
        }

        [Test]
        public void GetScore_AfterFirstRoll()
        {
            game.Roll(7);

            game.GetScore().Should().Be(7);
        }

        [Test]
        public void GetScore_AfterOneFrame()
        {
            game.Roll(6);
            game.Roll(3);

            game.GetScore().Should().Be(9);
        }

        [Test]
        public void GetScore_WithMultipleFrames()
        {
            game.Roll(6);
            game.Roll(3);
            game.Roll(2);

            game.GetScore().Should().Be(11);
        }

        [Test]
        public void GetScore_AfterSpare()
        {
            game.Roll(6);
            game.Roll(4);
            game.Roll(5);

            game.GetScore().Should().Be(20);
        }

        [Test]
        public void GetScore_AfterStrikeStrike()
        {
            game.Roll(10);
            game.Roll(10);

            game.GetScore().Should().Be(30);
        }

        [Test]
        public void GetScore_AfterMultipleStrikes()
        {
            game.Roll(10);
            game.Roll(10);
            game.Roll(10);

            game.GetScore().Should().Be(60);
        }

        [Test]
        public void GetScore_AfterSpareAndStrike()
        {
            game.Roll(10);
            game.Roll(6);
            game.Roll(4);
            game.Roll(5);

            game.GetScore().Should().Be(40);
        }

        [Test]
        public void PerfectGame_Returns300Points()
        {
            for(int i = 0; i < 12; i++)
                game.Roll(10);

            game.GetScore().Should().Be(300);
        }

        [Test]
        public void MoreThan10Frames_NoBonusGame_ThrowNotSupportedException()
        {
            for (int i = 0; i < 20; i++)
            {
                game.Roll(4);
            }

            Action a = () => game.Roll(5);
            a.ShouldThrow<NotSupportedException>();
        }

        [Test]
        public void AnotherThrowAfterBonusGameOver_ThrowNotSupportedException()
        {
            for (int i = 0; i < 12; i++)
            {
                game.Roll(10);
            }

            Action a = () => game.Roll(5);
            a.ShouldThrow<NotSupportedException>();
        }

        [Test]
        public void ScoreOfOneRoll()
        {

            game.Roll(2);
            game.GetScore().Should().Be(2);
        }

        [Test]
        public void ThreeRollsWithScoreBiggerThenTen_DontThrowException()
        {
            game.Roll(3);
            game.Roll(4);
            game.Roll(4);
            game.GetScore().Should().Be(11);
        }
        [Test]
        public void SpareAndOneRoll_ShouldAddDoublePointsForRoll()
        {
            game.Roll(7);
            game.Roll(3);
            game.Roll(5);
            game.GetScore().Should().Be(20);
        }
        [Test]
        public void StrikeAndTwoRolls_ShouldAddDoublePointsForRolls()
        {
            game.Roll(10);
            game.Roll(3);
            game.Roll(5);
            game.GetScore().Should().Be(26);
        }

        [Test]
        public void TripleStrike_ShouldWorkCorrect()
        {
            game.Roll(10);
            game.Roll(10);
            game.Roll(10);
            game.GetScore().Should().Be(60);
        }
        [Test]
        public void RollAfterTenFrames_ShouldThrowException()
        {
            for (int i = 0; i < 9; i++)
            {
                game.Roll(10);
            }
            game.Roll(0);
            game.Roll(0);
            Action a = () => game.Roll(1);
            a.ShouldThrow<NotSupportedException>();

        }
        [Test]
        public void TwoStrikesAfterLastFrameStrike_ShouldWorkCorrect()
        {
            for (int i = 0; i < 18; i++)
            {
                game.Roll(1);
            }
            game.Roll(10);
            game.Roll(10);
            game.Roll(10);

            game.GetScore().Should().Be(48);

        }
        [Test]
        public void RollAfterGameOver_ShouldThrowException()
        {
            for (int i = 0; i < 18; i++)
            {
                game.Roll(1);
            }
            game.Roll(10);
            game.Roll(10);
            game.Roll(10);
            Action a = () => game.Roll(10);
            a.ShouldThrow<NotSupportedException>();

        }
        [Test]
        public void OneRollAfterLastFrameSpare_ShouldWorkCorrect()
        {
            for (int i = 0; i < 18; i++)
            {
                game.Roll(1);
            }
            game.Roll(5);
            game.Roll(5);
            game.Roll(10);
            game.GetScore().Should().Be(38);
        }

        [Test]
        public void TwoRollsAfterLastFrameSpare_ShouldThrowException()
        {
            for (int i = 0; i < 18; i++)
            {
                game.Roll(1);
            }
            game.Roll(5);
            game.Roll(5);
            game.Roll(1);
            Action a = () => game.Roll(1);
            a.ShouldThrow<NotSupportedException>();

        }

        [Test]
        public void AllStrikes_ShouldReturnMaxScore()
        {
            for (int i = 0; i < 12; ++i)
            {
                game.Roll(10);
            }
            game.GetScore().Should().Be(300);
        }

        [Test]
        public void StupidTestFromMax()
        {
            game.Roll(10);
            game.Roll(5);
            game.Roll(5);
            game.Roll(5);

            game.GetScore().Should().Be(40);

        }
    }
}
