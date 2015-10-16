using NUnit.Framework;

namespace HashedWheelTimers.Tests
{
    [TestFixture]
    public class IsPowerOfTwoTests
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(8)]
        [TestCase(16)]
        [TestCase(32)]
        [TestCase(64)]
        [TestCase(128)]
        [TestCase(256)]
        [TestCase(2097152)]
        public void ReturnsTrueForPowersOfTwo(int input)
        {
            Assert.IsTrue(Utils.IsPowerOfTwo(input));
        }

        [TestCase(0)]
        [TestCase(127)]
        [TestCase(264)]
        [TestCase(-2)]
        public void ReturnsFalseWhenNotPowerOfTwo(int input)
        {
            Assert.IsFalse(Utils.IsPowerOfTwo(input));
        }
    }
}
