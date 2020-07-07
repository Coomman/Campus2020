using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace MockFramework
{
    public class ThingCache
    {
        private readonly IDictionary<string, Thing> dictionary = new Dictionary<string, Thing>();
        private readonly IThingService thingService;

        public ThingCache(IThingService thingService)
        {
            this.thingService = thingService;
        }

        public Thing Get(string thingId)
        {
            if (dictionary.TryGetValue(thingId, out var thing))
                return thing;

            if (!thingService.TryRead(thingId, out thing))
                return null;

            dictionary[thingId] = thing;
            return thing;
        }
    }

    [TestFixture]
    public class ThingCache_Should
    {
        private IThingService thingService;
        private ThingCache thingCache;

        private const string ThingId1 = "The Dress";
        private Thing thing1 = new Thing(ThingId1);

        private const string ThingId2 = "Cool Boots";
        private Thing thing2 = new Thing(ThingId2);

        public Thing MissingThing;

        [SetUp]
        public void SetUp()
        {
            thingService = A.Fake<IThingService>();
            thingCache = new ThingCache(thingService);
        }

        [Test]
        public void GetThing_FromEmptyCache_ReturnsNull()
        {
            thingCache.Get(ThingId1).Should().BeNull();

            A.CallTo(() => thingService.TryRead(ThingId1, out thing1))
                .MustHaveHappened();
        }

        [Test]
        public void GetThing_AfterAddingOneThing_ShouldCallTryReadOnceAndReturnThisThing()
        {
            A.CallTo(() => thingService.TryRead(ThingId1, out thing1))
                .Returns(true);

            thingCache.Get(ThingId1).Should().Be(thing1);

            A.CallTo(() => thingService.TryRead(ThingId1, out thing1))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void GetThing_AfterAlreadyHaveItRequested_ShouldCallTryReadOnce()
        {
            A.CallTo(() => thingService.TryRead(ThingId1, out thing1))
                .Returns(true);

            thingCache.Get(ThingId1);
            thingCache.Get(ThingId1);

            A.CallTo(() => thingService.TryRead(ThingId1, out thing1))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void GetThing_AfterGettingTwoDifferentThingsAndFirstAgain_ShouldCallTryReadTwice()
        {
            A.CallTo(() => thingService.TryRead(ThingId1, out thing1))
                .Returns(true);
            A.CallTo(() => thingService.TryRead(ThingId2, out thing2))
                .Returns(true);

            thingCache.Get(ThingId1);
            thingCache.Get(ThingId2);
            thingCache.Get(ThingId1);

            A.CallTo(() => thingService.TryRead(ThingId1, out thing1))
                .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => thingService.TryRead(ThingId2, out thing2))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void GetThing_GettingAbsentThingAfterAddingExisting_ShouldReturnNull()
        {
            A.CallTo(() => thingService.TryRead(ThingId1, out thing1))
                .Returns(true);
            A.CallTo(() => thingService.TryRead("Void", out MissingThing))
                .Returns(false);

            thingCache.Get(ThingId1);
            thingCache.Get("Void").Should().Be(null);

            A.CallTo(() => thingService.TryRead(ThingId1, out thing1))
                .MustHaveHappened(Repeated.Exactly.Once);
            A.CallTo(() => thingService.TryRead("Void", out MissingThing))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}