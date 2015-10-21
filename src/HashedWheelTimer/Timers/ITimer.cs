using System;

namespace HashedWheelTimers.Timers
{
    public interface ITimer : IDisposable
    {
        void Start();

        void Stop();
    }
}
