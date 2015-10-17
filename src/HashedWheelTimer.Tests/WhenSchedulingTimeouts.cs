using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace HashedWheelTimers.Tests
{
    [TestFixture]
    public class WhenSchedulingTimeouts
    {
        [Test]
        public void ShouldThrowForTimerOutsideBoundsOfWheel()
        {
            var wheelTimer = new HashedWheelTimer(16, 100);
            Assert.Throws<ArgumentOutOfRangeException>(() => wheelTimer.SetTimeout(2000, () => { }));
        }

        [Test]
        public void Should_return_a_handle()
        {
            var stopWatch = new Stopwatch();
            var expired = new List<long>();

            var wheelTimer = new HashedWheelTimer(16, 100);
            var handle = wheelTimer.SetTimeout(200, () =>
            {
                expired.Add(stopWatch.ElapsedMilliseconds);
                Console.WriteLine("Timeout expired");
            });

            wheelTimer.SetTimeout(1500, () =>
            {
                expired.Add(stopWatch.ElapsedMilliseconds);
                Console.WriteLine("Timeout expired");
            });

            wheelTimer.Start();
            stopWatch.Start();

            Thread.Sleep(1600);

            Assert.IsNotNull(handle);
            Assert.AreEqual(Guid.Empty, handle);
            Assert.AreEqual(2, expired.Count);
        }
    }
}
