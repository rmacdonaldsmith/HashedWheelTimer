using System;
using System.Threading.Tasks;

namespace HashedWheelTimers
{
    public class Timeout : ICancellable
    {
        private readonly long _deadline;
        private Status _state;
        private readonly Action _expirationAction;

        public Timeout(long deadline, Action expirationAction)
        {
            _deadline = deadline;
            _expirationAction = expirationAction;
            _state = Status.Waiting;
        }

        public long Deadline
        {
            get { return _deadline; }
        }

        public Status State
        {
            get { return _state; }
        }

        public void ExpireTimeout()
        {
            if(_state == Status.Waiting)
                Task.Run(_expirationAction);
        }

        public enum Status
        {
            Waiting,
            Cancelled,
            Expired
        }

        public void Cancel()
        {
            _state = Status.Cancelled;
        }
    }
}