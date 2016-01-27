using NUnit.Framework;
using System;

namespace OneCog.Core.Tests
{
    [TestFixture]
    public class FallibleShould
    {
        [Test]
        public void ReturnASuccessfulResultWhenBuiltFromValue()
        {
            IFallible<int> actual = Fallible.FromValue(314);

            Assert.That(actual.IsValue, Is.True);
            Assert.That(actual.IsError, Is.False);
        }

        [Test]
        public void HoldTheCorrectValueAWhenBuiltFromValue()
        {
            IFallible<int> actual = Fallible.FromValue(314);

            Assert.That(actual.Value, Is.EqualTo(314));
            Assert.That(actual.Error, Is.Null);
        }

        [Test]
        public void ReturnAnErrorResultWhenBuiltFromError()
        {
            IFallible<int> actual = Fallible.FromError<int>(new ArgumentException());

            Assert.That(actual.IsError, Is.True);
            Assert.That(actual.IsValue, Is.False);
        }

        [Test]
        public void HoldAnErrorAWhenBuiltFromError()
        {
            IFallible<int> actual = Fallible.FromError<int>(new ArgumentException());

            Assert.That(actual.Error, Is.InstanceOf<ArgumentException>());
            Assert.That(actual.Value, Is.EqualTo(default(int)));
        }
    }
}
