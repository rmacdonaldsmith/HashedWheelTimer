using System;
using System.Threading;
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

            var wheelTimer = new HashedWheelTimer(16, 100);
            var handle = wheelTimer.SetTimeout(200, () =>
            {
                expired = true;
                Console.WriteLine("Timeout expired");
            });

            wheelTimer.SetTimeout(1500, () =>
            {
                expired = true;
                Console.WriteLine("Timeout expired");
            });

            wheelTimer.Start();

            Thread.Sleep(1600);

            Assert.IsNotNull(handle);
            Assert.AreEqual(Guid.Empty, handle);
        }
    }
}
