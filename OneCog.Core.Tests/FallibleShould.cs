using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Core.Tests
{
    [TestFixture]
    public class FallibleShould
    {
        [Test]
        public void ReturnASuccessfulResultWhenBuiltFromValue()
        {
            IFallible<int> actual = Fallible.FromValue<int>(314);

            Assert.That(actual.IsValue, Is.True);
            Assert.That(actual.IsError, Is.False);
        }

        [Test]
        public void ReturnHoldValueAWhenBuiltFromValue()
        {
            IFallible<int> actual = Fallible.FromValue<int>(314);

            Assert.That(actual.Value, Is.EqualTo(314));
            Assert.That(actual.Error, Is.Null);
        }

        [Test]
        public void ReturnAErrorResultWhenBuiltFromError()
        {
            IFallible<int> actual = Fallible.FromError<int>(new ArgumentException());

            Assert.That(actual.IsError, Is.True);
            Assert.That(actual.IsValue, Is.False);
        }

        [Test]
        public void ReturnHoldErrorAWhenBuiltFromError()
        {
            IFallible<int> actual = Fallible.FromError<int>(new ArgumentException());

            Assert.That(actual.Error, Is.InstanceOf<ArgumentException>());
            Assert.That(actual.Value, Is.EqualTo(default(int)));
        }
    }
}
