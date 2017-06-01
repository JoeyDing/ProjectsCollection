using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LEAF.Plugins.ImageViewer
{
    public class ThrottleService
    {
        private Action lastAction;
        private DispatcherTimer timer = null;
        private readonly object locker = new object();

        public void Throttle(TimeSpan interval, Action action)
        {
            lock (locker)
            {
                lastAction = action;
                //first call, start timer
                if (timer == null)
                {
                    timer = new DispatcherTimer { Interval = interval };
                    timer.Tick += (sender, args) =>
                    {
                        timer.Stop();
                        timer = null;
                        lastAction();
                    };
                    timer?.Start();
                }
                else
                {
                    timer.Stop();
                    timer.Start();
                }
            }
        }
    }
}