using System;
using HashedWheelTimer.Core;

namespace HashedWheelTimers
{
    public class HashedWheelTimer
    {
        private readonly int _tickDuration;
        private readonly TimeBucket[] _timerWheel;

        public HashedWheelTimer(int ticksPerWheel, int tickDuration)
        {
            _tickDuration = tickDuration;
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

        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
