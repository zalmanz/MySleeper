using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MySleeper.Models;

namespace MySleeper
{

    public partial class MainWindow : Window
    {
        internal static SettingsTimers settingsTimers = new();
        internal static SettingsVisuals settingsVisuals = new();
        internal static List<BackGroundImages> backGroundImages = new();
        internal static MainWindow Instance { get; private set; }

        
        public MainWindow()
        {
            InitializeComponent();

            Instance = this;
            settingsTimers = DBContext.GetSettingsTimers();
            settingsVisuals = DBContext.GetSettingsVisuals();
            backGroundImages = DBContext.GetBackGroundImages();
            
            SetValuesMainWindow(settingsTimers, settingsVisuals, backGroundImages);
            this.Hide();
            Timers.TimerBreak();
        }

        

        internal static void SetValuesMainWindow(SettingsTimers st, SettingsVisuals sv, List<BackGroundImages> bg)
        {
            Color color = (Color)ColorConverter.ConvertFromString(sv.SV_BACKCOLOR);
            Instance.Background = new SolidColorBrush(color);
            color = (Color)ColorConverter.ConvertFromString(sv.SV_FONTCOLOR);

            if (bg.Count > 0)
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new(bg.First().BG_DATA);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();
                Instance.Background = new ImageBrush(biImg);
                Timers.RotateTimer(sv.SV_TIMERROTATEIMG);
            }
            Instance.Background.Opacity = sv.SV_OPACITY;

            Instance.labletextSleeper.Foreground = new SolidColorBrush(color);
            Instance.lableBreakTime.Foreground = new SolidColorBrush(color);
            Instance.lableBreakTimer.Foreground = new SolidColorBrush(color);
            Instance.lable1.Foreground = new SolidColorBrush(color);
            Instance.lable2.Foreground = new SolidColorBrush(color);
            Instance.labletextSleeper.Content = st.ST_TEXT;

            /*BreakTime = 0;
            BreakTime = Convert.ToInt32(st.ST_TIME) * 60;
            TimeSpan tstime = TimeSpan.FromSeconds(BreakTime);
            Instance.lableBreakTime.Content = $"{tstime.Hours} часов {tstime.Minutes} минут {tstime.Seconds} секунд";
            BreakTimer = 0;
            BreakTimer = Convert.ToInt32(st.ST_TIMER) * 60;
            tstime = TimeSpan.FromSeconds(BreakTimer);
            Instance.lableBreakTimer.Content = $"{tstime.Hours} часов {tstime.Minutes} минут {tstime.Seconds} секунд";*/
        }

        internal static void SetBackImagesWindow()
        {
            Random random = new();
            var a = backGroundImages.OrderBy(x => random.Next()).Take(1);
            BitmapImage biImg = new();
            MemoryStream ms = new(a.First().BG_DATA);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            Instance.Background = new ImageBrush(biImg)
            {
                Opacity = settingsVisuals.SV_OPACITY
            };
        }

        private void ButtonShowSetting_Click(object sender, RoutedEventArgs e)
        {
            
            Show();
            SettingWindow f = new SettingWindow();
            f.Owner = this;
            
            f.Closed += (object s,EventArgs args) =>
            {
                if (f.onSave)
                {
                    settingsVisuals = DBContext.GetSettingsVisuals();
                    settingsTimers = DBContext.GetSettingsTimers();
                    backGroundImages = DBContext.GetBackGroundImages();
                    
                    SetValuesMainWindow(settingsTimers, settingsVisuals, backGroundImages);
                }
            };
            f.ShowDialog();
        }
        private void TaskIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            this.Show();
        }
        private void TaskIcon_TrayLeftClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonHideMainWindow_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
        private void ButtonShowTraningWindow_Click(object sender, RoutedEventArgs e)
        {
            
            TraningWindow tr = new();
            tr.Topmost = true;
            tr.ShowDialog();
        }

        internal void OnSourceInitialized(object sender, EventArgs e)
        {
            HwndSource source = (HwndSource)PresentationSource.FromVisual(this);
            source.AddHook(new HwndSourceHook(HandleMessages));
        }

        internal IntPtr HandleMessages(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x0112 && ((int)wParam & 0xFFF0) == 0xF020)
            {
                handled = true;
                Hide();
            }
            return IntPtr.Zero;
        }
    }
}
