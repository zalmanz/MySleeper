using System;
using System.Windows.Threading;

namespace MySleeper
{
    public class Timers : MainWindow
    {
        //private static DispatcherTimer TimerRotate = new();
        private static DispatcherTimer TimerSleeper = new();
        private static bool t = true;
        internal static int BreakTimer;

        internal static void TimerBreak()
        {
            TimerSleeper.IsEnabled = true;
            TimerSleeper.Interval = new TimeSpan(0, 0, 1);
            TimerSleeper.Tick += timerBreakTimerTick;
            TimerSleeper.Start();
        }

        internal static void timerBreakTimerTick(object sender, EventArgs e)
        {
            BreakTimer--;
            
            if (BreakTimer <= 0 && !t)
            {
                BreakTimer = (int)settingsTimers.ST_TIME * 60;
                t = true;
                TimeSpan tstime = TimeSpan.FromSeconds((int)settingsTimers.ST_TIMER * 60);
                Instance.lableBreakTimer.Content = $"{tstime.Hours} часов {tstime.Minutes} минут {tstime.Seconds} секунд";
                Instance.Show();
                Instance.Topmost = true;
            }
            if (BreakTimer <= 0 && t)
            {
                BreakTimer = (int)settingsTimers.ST_TIMER * 60;
                t = false;
                TimeSpan tstime = TimeSpan.FromSeconds((int)settingsTimers.ST_TIME * 60);
                Instance.lableBreakTime.Content = $"{tstime.Hours} часов {tstime.Minutes} минут {tstime.Seconds} секунд";
                Instance.Hide();
            }
            TimeSpan ts = TimeSpan.FromSeconds(BreakTimer);

            if (BreakTimer > 0 && !t)
            {
                Instance.TaskBarText.Text = $"До перерыва {ts.Hours} часов {ts.Minutes} минут {ts.Seconds} секунд";
                Instance.lableBreakTimer.Content = $"{ts.Hours} часов {ts.Minutes} минут {ts.Seconds} секунд"; 
            }
            if (BreakTimer > 0 && t)
            {
                Instance.lableBreakTime.Content = string.Format("{0} часов {1} минут {2} секунд", ts.Hours, ts.Minutes, ts.Seconds);
            }
        }

        internal static void RotateTimer(long a)
        {
            DispatcherTimer TimerRotate = new();
            TimerRotate.Tick += new EventHandler(rotateBreakTimeTick);
            TimerRotate.Interval = new TimeSpan(0, 0, Convert.ToInt32(a));
            TimerRotate.Start();
        }
        internal static void rotateBreakTimeTick(object sender, EventArgs e)
        {
            SetBackImagesWindow();
        }
    }
}
