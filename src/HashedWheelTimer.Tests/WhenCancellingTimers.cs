using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace HashedWheelTimers.Tests
{
    [TestFixture]
    public class WhenCancelingTimers
    {
        [Test]
        public void ShouldThrowForUnknownTimers()
        {
            var wheelTimer = new HashedWheelTimer(16, 100);
            wheelTimer.SetTimeout(124, () => { });

            wheelTimer.Start();

            wheelTimer.CancelTimeout(Guid.Empty);
        }
    }
}
