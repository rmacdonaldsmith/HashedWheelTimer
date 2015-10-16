using System;
using HashedWheelTimer.Core;

namespace HashedWheelTimers
{
    public class HashedWheelTimer
    {
        private readonly TimeBucket[] _timerWheel;

        public HashedWheelTimer(int ticksPerWheel, int tickDuration)
        {
            Ensure.Nonnegative(ticksPerWheel, "ticksPerWheel");
            Ensure.Nonnegative(tickDuration, "tickDuration");

            _timerWheel = new TimeBucket[ticksPerWheel];
        }

        public Guid SetTimeout(DateTime deadline, Action expirationAction)
        {
            throw new NotImplementedException();
        }

        public void CancelTimeout(Guid handle)
        {

        }
    }
}
