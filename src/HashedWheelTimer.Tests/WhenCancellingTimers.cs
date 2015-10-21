using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace HashedWheelTimers.Tests
{
    [TestFixture]
    public class WhenCancelingTimers
    {
        [Test]
        public void ShouldCancelTheTimer()
        {
            var expired = false;
            var wheelTimer = new HashedWheelTimer(16, 100);
            var cancellation = wheelTimer.SetTimeout(200, () => expired = true);

            wheelTimer.Start();

            cancellation.Cancel();

            Thread.Sleep(300);

            Assert.IsFalse(expired);
        }

        [Test]
        public void ShouldOnlyCancelThisTimer()
        {
            var expired = new List<string>(3);
            var wheelTimer = new HashedWheelTimer(16, 100);
            var cancellation1 = wheelTimer.SetTimeout(200, () => expired.Add("1"));
            var cancellation2 = wheelTimer.SetTimeout(200, () => expired.Add("2"));
            var cancellation3 = wheelTimer.SetTimeout(200, () => expired.Add("3"));

            wheelTimer.Start();

            cancellation2.Cancel();

            Thread.Sleep(300);

            Assert.AreEqual(2, expired.Count);
            CollectionAssert.DoesNotContain(expired, "2");
        }
    }
}
