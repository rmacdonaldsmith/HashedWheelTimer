using System;
using System.Threading.Tasks;

namespace HashedWheelTimer.Core
{
    public class TimeoutElement
    {
        private readonly long _deadline;
        private readonly Action _expirationAction;

        public TimeoutElement(long deadline, Action expirationAction)
        {
            _deadline = deadline;
            _expirationAction = expirationAction;
        }

        public long Deadline
        {
            get { return _deadline; }
        }

        public Task ExpireTimeout()
        {
            return Task.Run(_expirationAction);
        }
    }
}
