using System;
using NUnit.Framework;

namespace HashedWheelTimers.Tests
{
    [TestFixture]
    public class WhenSchedulingTimeouts
    {
        [Test]
        public void Should_return_a_handle()
        {
            var now = DateTime.Now;
            var expired = false;

            var wheelTimer = new HashedWheelTimer();
            var handle = wheelTimer.SetTimeout(now, () =>
            {
                expired = true;
            });

            Assert.IsNotNull(handle);
            Assert.AreNotEqual(Guid.Empty, handle);
        }
    }
}
