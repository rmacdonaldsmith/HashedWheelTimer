using System.Collections.Generic;
using System.Linq;

namespace HashedWheelTimer.Core
{
    public class TimeBucket
    {
        //consider a dictionary here <guid, timeout> so that we get
        //constant time lookups when we want to cancel a timeout.
        private readonly HashSet<TimeoutElement> _timeouts;

        public TimeBucket() : this(10)
        {
        }

        public TimeBucket(int initialLength)
        {
            _timeouts = new HashSet<TimeoutElement>();
        }

        public void Add(TimeoutElement timeout)
        {
            _timeouts.Add(timeout);
        }

        /// <summary>
        /// Calls the ExpireTimer delegate for all timeouts in this bucket
        /// </summary>
        public void ExpireTimers()
        {
            if (!_timeouts.Any()) 
                return;

            foreach (var timeoutElement in _timeouts)
            {
                timeoutElement.ExpireTimeout();
            }

            _timeouts.Clear();
        }
    }
}
