using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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

namespace Calculation_Vacation
{
    /// <summary>
    /// Логика взаимодействия для Page_SWD.xaml
    /// </summary>
    public partial class Page_SWD : Window
    {
        private DataSaver _dataSaver;
        public readonly DependencyProperty TextProperty;

        public int TotalDayWorkInYers {  get; set; }

        private List<DataGridColumn> colum = new List<DataGridColumn>()
        {
          new DataGridTextColumn()
          {
                    Header = "Месяц работы",
                    Width = DataGridLength.SizeToHeader,
                    CanUserReorder = false,
                    CanUserResize = false,
                    CanUserSort = false,
                    FontSize = 13,
                    IsReadOnly = true,
                    Binding = new Binding("month")
          },
          new DataGridTextColumn()
          {
                    Header = "День заезда на вахту",
                    Width = DataGridLength.SizeToHeader,
                    CanUserReorder = false,
                    CanUserResize = false,
                    CanUserSort = false,
                    FontSize = 13,
                    IsReadOnly = false,
                    Binding = new Binding {Path = new PropertyPath("start"), StringFormat ="dd.MMMM"}

          },
          new DataGridTextColumn()
          {
                    Header = "День выезда с вахты",
                    Width = DataGridLength.SizeToHeader,
                    CanUserReorder = false,
                    CanUserResize = false,
                    CanUserSort = false,
                    FontSize = 13,
                    IsReadOnly = false,
                    Binding = new Binding{Path = new PropertyPath("stop"), StringFormat ="dd.MMMM"}
          },
          new DataGridTextColumn()
          {
                    Header = "Кол-во отработаных дней в месяце",
                    Width = DataGridLength.SizeToHeader,
                    CanUserReorder = false,
                    CanUserResize = false,
                    CanUserSort = false,
                    FontSize = 13,
                    IsReadOnly = true,
                    Binding = new Binding{Path = new PropertyPath("dayofwork"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, NotifyOnSourceUpdated = true }
          },
        };

        private BindingList<Month_Work> MW_ = new BindingList<Month_Work>();

        public class Month_Work : INotifyPropertyChanged, INotifyCollectionChanged
        {
            private DateTime? _start;
            private DateTime? _stop;
            private int _dayofwork;

            public string month { get; set; }

            public DateTime? start
            {
                get
                {
                    return _start;
                }
                set
                {
                    if (_start == value)
                    {
                        return;
                    }
                    _start = value;
                    PropertyIsChanged("start");
                }
            }

            public DateTime? stop
            {
                get
                {
                    return _stop;
                }
                set
                {
                    if (_stop == value)
                    {
                        return;
                    }
                    _stop = value;
                    PropertyIsChanged("stop");
                }
            }

            public int dayofwork
            {
                get
                {
                    return _dayofwork;
                }
                set
                {
                    if (_dayofwork == value)
                    {
                        return;
                    }
                    _dayofwork = value;
                    PropertyIsChanged("dayofwork");
                }
            }

            public Month_Work(string txt)
            {
                month = txt;
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            public event NotifyCollectionChangedEventHandler? CollectionChanged;

            private void PropertyIsChanged(string txt = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(txt));
            }

            private void CollectionIsChang(NotifyCollectionChangedAction e)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(e));
            }
        }

        public Page_SWD()
        {
            _dataSaver = new DataSaver();
            InitializeComponent();
            TextProperty = DependencyProperty.Register(
                    "Text1",
                    typeof(string),
                    typeof(TextBox),
                    new FrameworkPropertyMetadata(
                        string.Empty,
                        FrameworkPropertyMetadataOptions.AffectsMeasure |
                        FrameworkPropertyMetadataOptions.AffectsRender));
        }

        private void DataGrid_Set()
        {
            for (int i = 0; i < colum.Count; i++)
            {
                My_DataGrid.Columns.Add(colum[i]);
            }
        }

        private void DataGrid_Set_Wieth()
        {
            double wirth_1 = 0;
            for (int i = 0; i < this.My_DataGrid.Columns.Count; i++)
            {
                wirth_1 += this.My_DataGrid.Columns[i].ActualWidth;
            }
            this.My_DataGrid.Width = wirth_1 + 15;
        }

        private void My_DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid_Set_Wieth();
        }

        private void MW_ListChanged(object? sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                Txt_test.Text = "Информация записанна";
                DayWorkInMonth(sender);
                TotalDayWork(sender);
                _dataSaver.SaveDate(MW_);
                ShowTotalDay.Text = $"Общее количество отработаных дней в году: {TotalDayWorkInYers.ToString()}";
            }
        }

        private void MW_ColletionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Replace)
            {
                Txt_test.Text = "Произошло что-то что не понятно";
            }
        }

        private void My_DataGrid_Init(object sender, EventArgs e)
        {
            Month_Work_Set();
            DataGrid_Set();
            MW_.ListChanged += MW_ListChanged;
        }

        private void MW__ListChanged(object? sender, ListChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Month_Work_Set()
        {
            if (_dataSaver.TableIsCreate)
            {
                MW_ = _dataSaver.LoadDate();
                My_DataGrid.ItemsSource = MW_;
            }
            else
            {
                DateTime nowMonth = DateTime.Now;
                DateOnly dateOnly = new DateOnly(2024, 01, 01);
                var test = dateOnly;

                for (int i = 1; i <= nowMonth.Month; i++)
                {
                    MW_.Add(new Month_Work(test.ToString("MMMM")));
                    test = dateOnly.AddMonths(i);
                }
                My_DataGrid.ItemsSource = MW_;
            }
        }

        private void My_DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
        }

        private void My_DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var tt = sender as DataGrid;
            var r = e.EditingElement.Parent;
            var q = r.ReadLocalValue(TextProperty);

        }

        private void My_DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
        }

        private void Butt_For_Delet_JSON_Click(object sender, RoutedEventArgs e)
        {
            _dataSaver.DeletData();
            Txt_test.Text = "База данных сброшена";
        }

        private void Butt_to_Save_JSON_Click(object sender, RoutedEventArgs e)
        {
            _dataSaver.SaveDate(MW_);
            Txt_test.Text = "Сохранение прошло удачно";
        }

        private void DayWorkInMonth(object? sender)
        {
            var Month_Work = sender as BindingList<Month_Work>;
            foreach (var month in Month_Work)
            {
                if (month.start != null && month.stop != null)
                {
                    TimeSpan? diff = month.stop - month.start;
                    month.dayofwork =  (bool)OneDayPlus.IsChecked ? ( diff.Value.Days + 1) : ( diff.Value.Days);
                }
            }

        }

        private void TotalDayWork(object? sender)
        {
            var Month_Work = sender as BindingList<Month_Work>;
            int day = 0;
            foreach (var month in Month_Work)
            {
                day += month.dayofwork;
            }
            TotalDayWorkInYers = day;
        }

        private void OneDayPlus_Click(object sender, RoutedEventArgs e)
        {
            DayWorkInMonth(this.MW_);
        }

        private void Show_total_txt_Loaded(object sender, RoutedEventArgs e)
        {
            TotalDayWork(this.MW_);
            ShowTotalDay.Text = $"Общее количество отработаных дней в году: {TotalDayWorkInYers.ToString()}";
        }
    }
}
