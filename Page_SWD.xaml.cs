using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
                    Binding = new Binding("dayofwork")
          },
        };

        private BindingList<Month_Work> MW_ = new BindingList<Month_Work>();

        public class Month_Work : INotifyPropertyChanged
        {
            private DateTime? _start;
            private DateTime? _stop;

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
                    OnpropertiChang("start");
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
                    OnpropertiChang("stop");
                }
            }

            public int dayofwork { get; }

            public Month_Work(string txt)
            {
                month = txt;
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            private void OnpropertiChang(string txt = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(txt));
            }
        }

        public Page_SWD()
        {
            _dataSaver = new DataSaver();
            InitializeComponent();

        }

        private void DataGrid_Set()
        {
            for (int i = 0; i < colum.Count; i++)
            {
                My_DataGrid.Columns.Add(colum[i]);
            }
        }

        public void DataGrid_Set_Wieth()
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
            }
        }

        private void My_DataGrid_Init(object sender, EventArgs e)
        {

            Month_Work_Set();
            DataGrid_Set();
            MW_.ListChanged += MW_ListChanged;
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

        private int DayWorkInMonth (DateTime dt1,  DateTime dt2)
        {
            var daywork = dt2 - dt1;
            int day = daywork.Days;
            return day;
        }
    }
}
