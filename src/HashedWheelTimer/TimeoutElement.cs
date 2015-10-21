using System;
using System.Threading.Tasks;

namespace HashedWheelTimers
{
    public interface ICancellable
    {
        void Cancel();
    }

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

        public Task ExpireTimeout()
        {
            return Task.Run(_expirationAction);
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
