using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HashedWheelTimers.Timers
{
    //taken from here: http://web.archive.org/web/20110910100053/http://www.indigo79.net/archives/27#comment-255
    //depends on a windows dll, so not going to persue this at the moment.
    public class MultiMediaBasedTimer : ITimer
    {
        public delegate void ElapsedTimerDelegate(int tick, TimeSpan span);
        private readonly ElapsedTimerDelegate _elapsedTimerHandler;
        private readonly int _interval;

        public MultiMediaBasedTimer(int intervalMs, ElapsedTimerDelegate callback)
        {
            _interval = intervalMs;
            _elapsedTimerHandler = callback;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            timeBeginPeriod(1);
            mHandler = new TimerEventHandler(TimerHandler);
            mTimerId = timeSetEvent(_interval, 0, mHandler, IntPtr.Zero, EVENT_TYPE);
            mTestStart = DateTime.Now;
            mTestTick = 0;
        }

        public void Stop()
        {
            int err = timeKillEvent(mTimerId);
            timeEndPeriod(1);
            mTimerId = 0;
        }

        private void TimerHandler(int id, int msg, IntPtr user, int dw1, int dw2)
        {

            //_elapsedTimerHandler();

        }

        
        private int mTimerId;
        private TimerEventHandler mHandler;
        private int mTestTick;
        private DateTime mTestStart;

        // P/Invoke declarations
        private delegate void TimerEventHandler(int id, int msg, IntPtr user, int dw1, int dw2);

        private const int TIME_PERIODIC = 1;
        private const int EVENT_TYPE = TIME_PERIODIC; // + 0x100;  // TIME_KILL_SYNCHRONOUS causes a hang ?!

        [DllImport("winmm.dll")]
        private static extern int timeSetEvent(int delay, int resolution,
                                               TimerEventHandler handler, IntPtr user, int eventType);

        [DllImport("winmm.dll")]
        private static extern int timeKillEvent(int id);

        [DllImport("winmm.dll")]
        private static extern int timeBeginPeriod(int msec);

        [DllImport("winmm.dll")]
        private static extern int timeEndPeriod(int msec);

    }
}
