using System.Collections.Generic;
using System.Linq;

namespace HashedWheelTimers
{
    public class HashedWheelTimerBucket
    {
        //consider a dictionary here <guid, timeout> so that we get
        //constant time lookups when we want to cancel a timeout.
        private readonly HashSet<Timeout> _timeouts;

        public HashedWheelTimerBucket() : this(10)
        {
        }

        public HashedWheelTimerBucket(int initialLength)
        {
            _timeouts = new HashSet<Timeout>();
        }

        public void Add(Timeout timeout)
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
