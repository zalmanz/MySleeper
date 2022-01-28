using System;
using System.Windows.Threading;

namespace MySleeper
{
    public class Timers : MainWindow
    {
        private static readonly DispatcherTimer timerTimer = new();
        private static readonly DispatcherTimer timerTime = new();
        private static readonly DispatcherTimer TimerRotate = new();

        internal static void timerBreakTimer(bool status)
        {
            if (status)
            {
                timerTimer.Tick += new EventHandler(timerBreakTimerTick);
                timerTimer.Interval = new TimeSpan(0, 0, 1);
                timerTimer.Start();
            }
            else timerTimer.Stop();
        }
        internal static void timerBreakTimerTick(object sender, EventArgs e)
        {
            BreakTimer--;
            if (BreakTimer <= 0)
            {
                SetValuesMainWindow(settingsTimers, settingsVisuals, backGroundImages);
                timerBreakTimer(false);
                timerBreakTime(true);
                MainWindow.Instance.Show();
                MainWindow.Instance.Topmost = true;
            }
            else
            {
                TimeSpan ts = TimeSpan.FromSeconds(BreakTimer);
                MainWindow.Instance.lableBreakTimer.Content = $"{ts.Hours} часов {ts.Minutes} минут {ts.Seconds} секунд";
                MainWindow.Instance.TaskBarText.Text = $"До перерыва {ts.Hours} часов {ts.Minutes} минут {ts.Seconds} секунд";
            }
        }

        internal static void timerBreakTime(bool status)
        {
            if (status)
            {
                timerTime.Tick += new EventHandler(timeBreakTimeTick);
                timerTime.Interval = new TimeSpan(0, 0, 1);
                timerTime.Start();
            }
            else
            {
                timerTime.Stop();
            }
        }

        internal static void timeBreakTimeTick(object sender, EventArgs e)
        {
            BreakTime--;
            if (BreakTime <= 0)
            {
                SetValuesMainWindow(settingsTimers, settingsVisuals, backGroundImages);
                timerBreakTime(false);
                timerBreakTimer(true);
                MainWindow.Instance.Hide();
            }
            else
            {
                TimeSpan ts = TimeSpan.FromSeconds(BreakTime);
                MainWindow.Instance.lableBreakTime.Content = string.Format("{0} часов {1} минут {2} секунд", ts.Hours, ts.Minutes, ts.Seconds);
            }
        }

        internal static void RotateTimer(long a)
        {
            TimerRotate.Tick += new EventHandler(rotateBreakTimeTick);
            TimerRotate.Interval = new TimeSpan(0, 0, Convert.ToInt32(a));
            TimerRotate.Start();
        }
        internal static void rotateBreakTimeTick(object sender, EventArgs e)
        {
            SetBackImagesWindow();
            //Instance.Background.
        }

        
    }

}
