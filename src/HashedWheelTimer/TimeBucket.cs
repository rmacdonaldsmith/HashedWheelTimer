using System.Collections.Generic;
using System.Linq;

namespace HashedWheelTimer.Core
{
    public class TimeBucket
    {
        private readonly List<TimeoutElement> _timeouts;

        public TimeBucket() : this(10)
        {
        }

        public TimeBucket(int initialLength)
        {
            _timeouts = new List<TimeoutElement>(initialLength);
        }

        public void ExpireTimers()
        {
            if (!_timeouts.Any()) 
                return;

            foreach (var timeoutElement in _timeouts)
            {
                timeoutElement.ExpireTimeout();
            }
        }
    }
}
