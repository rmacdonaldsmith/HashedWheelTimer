using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace HashedWheelTimers
{
    public interface ITimer : IDisposable
    {
        void Start();

        void Stop();
    }

    public class ThreadingBasedTimer : ITimer, IDisposable
    {
        private readonly int _startsInMs;
        private readonly int _periodMs;
        private readonly Timer _timer;
        private readonly TimerCallback _timerCallback;
        private GCHandle _gcHandle;
        private bool _disposed;

        public ThreadingBasedTimer(int startsInMs, int periodMs, Action<object> callback)
        {
            _startsInMs = startsInMs;
            _periodMs = periodMs;
            _timerCallback = state => callback(state);
            _timer = new Timer(_timerCallback, null, System.Threading.Timeout.Infinite,
                System.Threading.Timeout.Infinite);
            _gcHandle = GCHandle.Alloc(_timerCallback);
        }

        public void Start()
        {
            _timer.Change(_startsInMs, _periodMs);
        }

        public void Stop()
        {
            _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Stop();
                    _gcHandle.Free();
                }
            }

            _disposed = true;
        }

        ~ThreadingBasedTimer()
        {
            Dispose(false);
        }
    }
}
