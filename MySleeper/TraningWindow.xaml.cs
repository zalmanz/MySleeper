using MySleeper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MySleeper
{

    public partial class TraningWindow : Window
    {
        private List<TypeTrening> tt = new();
        private List<SettingsTrenings> st = new();
        private ObservableCollection<DataGridAddTraning> obsAddTraning = new();
        public TraningWindow()
        {
            InitializeComponent();
            CurrentTime();

            st = DBContext.GetSettingsTrenings();
            tt = DBContext.GetTypeTrening();

            TypeTr.SelectedValuePath = "Key";
            TypeTr.ItemsSource = tt;

            ListCollectionView collectionView = new ListCollectionView(setDataDategrid());
            collectionView.SortDescriptions.Add(new SortDescription("DateTr1", ListSortDirection.Descending));
            collectionView.GroupDescriptions.Add(new PropertyGroupDescription("DateTr1"));
            HistoryTraning.ItemsSource = collectionView;



        }

        private ObservableCollection<DataGridAddTraning> setDataDategrid()
        {
            foreach (SettingsTrenings a in st)
            {
                string typetr = "";
                foreach (TypeTrening type in tt)
                {
                    if (type.TT_ID == a.STR_TYPE)
                    {
                        typetr = type.TT_NAME;
                    }
                }
                obsAddTraning.Add(new DataGridAddTraning {
                    DateTr1 = a.STR_DATE.ToString("yyyy-MM-dd"),
                    TimeTr1 = a.STR_DATE.ToString("HH:mm"),
                    TypeTr1 = typetr,
                    CountW1 = a.STR_NUMWORKOUT,
                    CountR1 = a.STR_NUMREPEAT
                });
            }
            
            return obsAddTraning;
        }

        private void SaveTraning(object sender, RoutedEventArgs e)
        {
            if ((string.IsNullOrEmpty(CountW.Text)) || (string.IsNullOrEmpty(CountR.Text)))
            {
                MessageBox.Show("Ведите количество подходов и повторений");
            }
            else
            {
                TypeTrening typeItem = (TypeTrening)TypeTr.SelectedItem;

                DBContext.SetSettingsTrenings(typeItem.TT_ID, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), Convert.ToInt32(CountW.Text), Convert.ToInt32(CountR.Text));
                obsAddTraning.Add(new DataGridAddTraning { DateTr1 = DateTime.Now.ToString("yyyy-MM-dd"), TimeTr1 = DateTime.Now.ToString("HH:mm"), TypeTr1 = typeItem.TT_NAME, CountW1 = Convert.ToInt32(CountW.Text), CountR1 = Convert.ToInt32(CountR.Text) });
                HistoryTraning.Items.Refresh();
            }
        }

        private void CountW_KeyPress(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        public void CurrentTime()
        {
            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += Timer_Tick;
            LiveTime.Start();
        }
        void Timer_Tick(object sender, EventArgs e)
        {
            CurentDateTime.Content = $"Тренировка на сегодня: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }

        

    }
}
