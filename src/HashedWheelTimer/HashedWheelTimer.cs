using System;
using System.Diagnostics;
using System.Threading;

namespace HashedWheelTimers
{
    public class HashedWheelTimer : IDisposable
    {
        private readonly int _ticksPerWheel;
        private readonly int _tickDurationMs;
        private readonly HashedWheelTimerTimeBucket[] _timerWheel;
        private ITimer _timer;
        private int _running = 0;
        private int _currentWheelIndex = 0;
        private readonly object _wheelLock = new object();
        private readonly Func<int, int, int> CalculateIndexModulo = (i, N) => i%N;
        private readonly Func<int, int, int> CalculateIndexBitwiseAnd = (i, tableSize) => i & (tableSize - 1);

        public HashedWheelTimer(int ticksPerWheel, int tickDurationMs)
        {
            Utils.Ensure.Nonnegative(ticksPerWheel, "ticksPerWheel"); 
            Utils.Ensure.Nonnegative(tickDurationMs, "tickDurationMs");
            Utils.Ensure.True(Utils.IsPowerOfTwo(ticksPerWheel),
                              string.Format("ticksPerWheel {0} needs to be a power of 2.", ticksPerWheel));

            _ticksPerWheel = ticksPerWheel;
            _tickDurationMs = tickDurationMs;
            _timerWheel = InitWheel(ticksPerWheel);
        }

        private HashedWheelTimerTimeBucket[] InitWheel(int ticksPerWheel)
        {
            //we need to make sure that the tickePerWheel is a power of 2
            //if not maybe we can shift the tickerPerWheel to the nearest power of 2?

            var wheel = new HashedWheelTimerTimeBucket[ticksPerWheel];

            for (int i = 0; i < ticksPerWheel; i++)
            {
                wheel[i] = new HashedWheelTimerTimeBucket();
            }

            return wheel;
        }

        public ICancellable SetTimeout(int delayMs, Action expirationAction)
        {
            //do not accept a timeout less than the wheel resolution
            if(delayMs <= _tickDurationMs)
                throw new ArgumentOutOfRangeException("delayMs", delayMs,
                                                      string.Format(
                                                          "Requested deadline {0}ms is less than the resolution of the wheel {1}ms",
                                                          delayMs, _tickDurationMs));

            //do not accept a timeout greater than the time represented by the wheel.
            if (delayMs > _ticksPerWheel*_tickDurationMs)
                throw new ArgumentOutOfRangeException("delayMs", delayMs,
                                                      string.Format(
                                                          "Requested deadline {0}ms is greater than the size of the wheel {1}ms",
                                                          delayMs, _ticksPerWheel*_tickDurationMs));

            var timerAdjustedDelay = delayMs/_tickDurationMs;
            var index = CalculateIndexBitwiseAnd(timerAdjustedDelay, _ticksPerWheel);
            var timeout = new Timeout(delayMs, expirationAction);

            lock (_wheelLock)
            {
                _timerWheel[index].Add(timeout);
            }

            return timeout;
        }

        public void Start()
        {
            Interlocked.CompareExchange(ref _running, 1, 0);
             
            _timer = new ThreadingBasedTimer(_tickDurationMs, _tickDurationMs, AdvanceWheel);
        }

        private void AdvanceWheel(object state)
        {
            if (!Running)
                return;

            if (_currentWheelIndex++ > _ticksPerWheel -1)
                _currentWheelIndex = 0;

            Debug.WriteLine("Advancing wheel position to {0}", _currentWheelIndex);

            //we probably dont need this lock if we can assert that inserts dont happen in to the current bucket
            lock (_wheelLock)
            {
                //i think this is naieve; we probably want to move the expired timeout actions to a queue and have a seperate thread handle them
                //so that we dont block the wheel from rotating.
                _timerWheel[_currentWheelIndex].ExpireTimers();
            }
        }

        public void Stop()
        {
            Interlocked.Decrement(ref _running);
            _timer.Stop();
        }

        protected bool Running {
            get { return _running == 1; }
        }

        private int RoundUpWheelSizeToPowerOf2(int initialSize)
        {
            var newSize = 0;
            while (newSize < initialSize)
                newSize = initialSize << 1;

            return newSize;
        }

        public void Dispose()
        {
            Stop();
            _timer.Dispose();
        }
    }
}
