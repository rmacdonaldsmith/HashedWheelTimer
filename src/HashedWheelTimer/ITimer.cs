using System;

namespace HashedWheelTimers
{
    public interface ITimer
    {
        void FiresIn(int milliseconds, Action callback);
    }
}
