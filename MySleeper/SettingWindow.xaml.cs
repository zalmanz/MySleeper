using Microsoft.Win32;
using MySleeper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MySleeper
{
    /// <summary>
    /// Логика взаимодействия для SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        private List<BackGroundImages> sbg = new();
        public bool onSave = false;
        public SettingWindow()
        {
            InitializeComponent();
            SettingsProgramm sp = DBContext.GetSettingsProgramm();
            SettingsTimers a = DBContext.GetSettingsTimers();
            SettingsVisuals sv = DBContext.GetSettingsVisuals();
            AddAutostart.IsChecked = CheckAutorunValue();
            //var aa = SetAutorunValue(false, false);
            AddAutostart.Click += (o, e) => SetAutorunValue(o, e, AddAutostart.IsChecked.Value);
            //AddAutostart.Checked += (o, e) => SetAutorunValue(o, e, AddAutostart.IsChecked.Value); //SetAutorunValue(AddAutostart.IsChecked);
            //AddAutostart.Click += (o, e) => SetAutorunValue(o, e, AddAutostart.IsChecked.Value, true); //SetAutorunValue(AddAutostart.IsChecked);
            text.Text = a.ST_TEXT;
            time.Text = a.ST_TIME.ToString();
            timer.Text = a.ST_TIMER.ToString();
            BGFontColor.Text = sv.SV_FONTCOLOR.ToString();
            BGFon.Text = sv.SV_BACKCOLOR.ToString();
            BGOpacity.Value = sv.SV_OPACITY;
            TimerRotaleImages.Text = sv.SV_TIMERROTATEIMG.ToString();
            
            if (sv.SV_BACKGROUNDisEnabled)
            {
                BACKGROUNDisEnabled.IsChecked = true;
                InsertImgsData();


            }
            else { BACKGROUNDisEnabled.IsChecked = false; AddBgImageButton.Visibility = Visibility.Hidden; }
        }

        private void InsertImgsData()
        {
            sbg = DBContext.GetBackGroundImages();

            if (StackPanelImages1.Children.Count > 0)
            {
                StackPanelImages1.Children.RemoveRange(0, StackPanelImages1.Children.Count);
            }
            

            if (sbg.Count > 0)
            {

                foreach (BackGroundImages imgs in sbg)
                {
                    
                    Image BGImage1 = new();
                    BGImage1.Height = 100;
                    BGImage1.Width = 100;
                    BGImage1.HorizontalAlignment = HorizontalAlignment.Left;
                    BGImage1.Margin = new Thickness(10, 0, 0, 0);
                    //BGImage1.VerticalAlignment = VerticalAlignment.Top;
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(imgs.BG_DATA);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    
                    BGImage1.Source = biImg;
                    StackPanelImages1.Children.Add(BGImage1);
                    
                    Button bt = new();
                    bt.Content = "-";
                    bt.ToolTip = "Удалить картинку";
                    bt.Height = 20;
                    bt.Width = 20;
                    
                    bt.VerticalAlignment = VerticalAlignment.Center;
                    bt.Margin = new Thickness(10, 0, 0, 0);
                    bt.Click += (o, e) => RemoveImg(o, e, imgs.BG_ID);
                    StackPanelImages1.Children.Add(bt);
                }
            }
        }

        private void RemoveImg(object sender, RoutedEventArgs e,long id)
        {
            
            //MessageBox.Show($"Удаляем картинку с id {id}");
            DBContext.RemoveImg(id);
            InsertImgsData();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            int timer = 0, time = 0, timerRotaleImages=0;
            
            string bgFontColor, bgFon;
            
            string err = "";
            try
            {
                bgFon = BGFon.Text;
                bgFontColor = BGFontColor.Text;
                timerRotaleImages = Convert.ToInt32(this.TimerRotaleImages.Text);
                DBContext.SetSettingsVisuals(bgFontColor, bgFon, Math.Round(BGOpacity.Value, 2), BACKGROUNDisEnabled.IsChecked.Value, timerRotaleImages);

                timer = Convert.ToInt32(this.timer.Text);
                time = Convert.ToInt32(this.time.Text);
                DBContext.SetSettingsTimers(this.text.Text, time, timer);
            }
            catch (Exception error)
            {
                err = error.Message.ToString();
                MessageBox.Show("Не верные данные");
            }

            if (string.IsNullOrEmpty(err))
            {
                onSave = true;
                Close();
            }
        }

        private void UploadImg(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                Image photo = new();
                photo.Source = new BitmapImage(new Uri(op.FileName));
                byte[] a = getJPGFromImageControl(new BitmapImage(new Uri(op.FileName)));
                DBContext.SetInsertBackGroundImages(a);
                InsertImgsData();
            }
        }
        private byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public bool SetAutorunValue(object sender, RoutedEventArgs e, bool autorun)
        {
            const string name = "MySleeper";
            string ExePath = System.Windows.Forms.Application.ExecutablePath;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                {
                    reg.SetValue(name, ExePath);
                    MessageBox.Show("Программа добавлена в автозагрузку");
                }
                else
                {
                    reg.DeleteValue(name);
                    MessageBox.Show("Программа удалена из автозагрузки");
                }
                reg.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool CheckAutorunValue()
        {
            const string name = "MySleeper";
            string ExePath = System.Windows.Forms.Application.ExecutablePath;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            if (reg.GetValue(name) == null)
            {
                return false;
            }
            else return true;
        }
    }
}
