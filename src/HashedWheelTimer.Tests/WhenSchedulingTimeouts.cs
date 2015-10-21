using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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
        public void ShouldThrowForTimerLessThanWheelResolution()
        {
            var wheelTimer = new HashedWheelTimer(16, 100);
            Assert.Throws<ArgumentOutOfRangeException>(() => wheelTimer.SetTimeout(99, () => { }));
        }

        [Test]
        public void Should_expire_timers_ontime()
        {
            var stopWatch = new Stopwatch();
            var expired = new List<long>();

            var wheelTimer = new HashedWheelTimer(16, 100);
            wheelTimer.Start();
            stopWatch.Start();

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

            Thread.Sleep(1600);
            wheelTimer.Stop();

            Assert.IsNotNull(handle);
            Assert.AreEqual(2, expired.Count);
            expired.Aggregate(0, (i, l) =>
            {
                Console.WriteLine("{0} {1}ms", i, l);
                return ++i;
            });
        }

        [Test]
        public void ManyTimeoutsWithinTheSameBucketShouldFireTogether()
        {
            var stopWatch = new Stopwatch();
            var expired = new List<long>();

            var wheelTimer = new HashedWheelTimer(16, 100);
            wheelTimer.SetTimeout(200, () => expired.Add(stopWatch.ElapsedMilliseconds));
            wheelTimer.SetTimeout(200, () => expired.Add(stopWatch.ElapsedMilliseconds));
            wheelTimer.SetTimeout(200, () => expired.Add(stopWatch.ElapsedMilliseconds));
            wheelTimer.SetTimeout(200, () => expired.Add(stopWatch.ElapsedMilliseconds));

            wheelTimer.Start();
            stopWatch.Start();

            Thread.Sleep(1600);

            Assert.AreEqual(4, expired.Count);

            expired.Aggregate(0, (i, l) =>
            {
                Console.WriteLine("{0} {1}ms", i, l);
                return ++i;
            });

            var expiredAt = expired.First();
            Assert.AreEqual(4, expired.Count(t => Math.Abs(t - expiredAt) < 3));
        }

        public abstract class WheelTimerSpec
        {
            protected Stopwatch Stopwatch = new Stopwatch();
            protected List<long> ExpiredTimeouts = new List<long>();
            protected HashedWheelTimer WheelTimer;

            public virtual void Init()
            {
                WheelTimer = new HashedWheelTimer(16, 100);
            }

            public void StartTimer()
            {
                Stopwatch.Start();
            }

            protected void OnTimerExpired()
            {
                ExpiredTimeouts.Add(Stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
