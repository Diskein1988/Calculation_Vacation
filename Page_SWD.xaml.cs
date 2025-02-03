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

        public int TotalDayWorkInYears { get; set; }

        private List<DataGridColumn> columns = new List<DataGridColumn>()
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
                    Binding = new Binding {
                        Path = new PropertyPath("start"),
                        StringFormat ="dd.MMMM",
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged}

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
                    Binding = new Binding{
                        Path = new PropertyPath("stop"),
                        StringFormat ="dd.MMMM",
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged}
          },
          new DataGridTextColumn()
          {
                    Header = "Кол-во отработанных дней в месяце",
                    Width = DataGridLength.SizeToHeader,
                    CanUserReorder = false,
                    CanUserResize = false,
                    CanUserSort = false,
                    FontSize = 13,
                    IsReadOnly = true,
                    Binding = new Binding{
                        Path = new PropertyPath("dayofwork"),
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                        NotifyOnSourceUpdated = true }
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
                    if ( _start == value )
                    {
                        return;
                    }
                    _start = value;
                    PropertyIsChanged( "start" );
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
                    if ( _stop == value )
                    {
                        return;
                    }
                    _stop = value;
                    PropertyIsChanged( "stop" );
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
                    if ( _dayofwork == value )
                    {
                        return;
                    }
                    _dayofwork = value;
                    PropertyIsChanged( "dayofwork" );
                    CollectionIsChang( NotifyCollectionChangedAction.Reset );
                }
            }

            public Month_Work( string txt )
            {
                month = txt;
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            public event NotifyCollectionChangedEventHandler? CollectionChanged;

            private void PropertyIsChanged( string txt = "" )
            {
                PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( txt ) );
            }

            private void CollectionIsChang( NotifyCollectionChangedAction e )
            {
                CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( e ) );
            }
        }

        public Page_SWD()
        {
            _dataSaver = new DataSaver();
            InitializeComponent();
        }

        private void DataGrid_Set()
        {
            for ( int i = 0; i < columns.Count; i++ )
            {
                My_DataGrid.Columns.Add( columns[i] );
            }
        }

        private void DataGrid_Set_Width()
        {
            double wirth_1 = 0;
            for ( int i = 0; i < this.My_DataGrid.Columns.Count; i++ )
            {
                wirth_1 += this.My_DataGrid.Columns[i].ActualWidth;
            }
            this.My_DataGrid.Width = wirth_1 + 13;
        }

        private void My_DataGrid_Loaded( object sender, RoutedEventArgs e )
        {
            DataGrid_Set_Width();
        }

        private void MW_ListChanged( object? sender, ListChangedEventArgs e )
        {
            if ( e.ListChangedType == ListChangedType.ItemChanged )
            {
                Txt_test.Text = "Информация записана";
                DayWorkInMonth( sender );
                TotalDayWork( sender );
                _dataSaver.SaveDate( MW_ );
                ShowTotalDay.Text = $"Общее количество отработанных дней в году: {TotalDayWorkInYears.ToString()}";
            }
        }

        private void MW_CollectionChanged( object? sender, NotifyCollectionChangedEventArgs e )
        {
            if ( e.Action == NotifyCollectionChangedAction.Reset )
            {
                Txt_test.Text = "Произошло что-то что не понятно";
            }
        }

        private void My_DataGrid_Init( object sender, EventArgs e )
        {
            Month_Work_Set( MainWindow.GetInstance.Start_work );
            DataGrid_Set();
            MW_.ListChanged += MW_ListChanged;
            //MW_[0].CollectionChanged += MW_CollectionChanged;
        }

        private void MW__ListChanged( object? sender, ListChangedEventArgs e )
        {
            throw new NotImplementedException();
        }

        private void Month_Work_Set( DateTime date )
        {
            DateTime nowMonth = DateTime.Now;

            if ( _dataSaver.TableIsCreate )
            {
                BindingList<Month_Work> temp_MW = _dataSaver.LoadDate();

                if ( temp_MW.Count == nowMonth.Month )
                {
                    My_DataGrid.ItemsSource = MW_ = temp_MW;
                }

                else if ( temp_MW.Count < nowMonth.Month )
                {
                    int diff = nowMonth.Month - temp_MW.Count;
                    DateTime dateOnly = new DateTime( nowMonth.Year, temp_MW.Count + 1, 1 );

                    for ( int i = 0; i < diff; i++ )
                    {
                        temp_MW.Add( new Month_Work( dateOnly.ToString( "MMMM" ) ) );
                        dateOnly = dateOnly.AddMonths( 1 );
                    }

                    My_DataGrid.ItemsSource = MW_ = temp_MW;
                }

            }
            else
            {
                for ( int i = 1; i <= nowMonth.Month; i++ )
                {
                    MW_.Add( new Month_Work( date.ToString( "MMMM" ) ) );
                    date = date.AddMonths( 1 );
                }
                My_DataGrid.ItemsSource = MW_;
            }
        }

        private void Butt_For_Delete_JSON_Click( object sender, RoutedEventArgs e )
        {
            _dataSaver.DeletData();
            Txt_test.Text = "База данных сброшена";
        }

        private void Butt_to_Save_JSON_Click( object sender, RoutedEventArgs e )
        {
            _dataSaver.SaveDate( MW_ );
            Txt_test.Text = "Сохранение прошло удачно";
        }

        private void DayWorkInMonth( object? sender )
        {
            var Month_Work = sender as BindingList<Month_Work>;
            foreach ( var month in Month_Work )
            {
                if ( month.start != null && month.stop != null )
                {
                    TimeSpan? diff = month.stop - month.start;
                    month.dayofwork = (bool)OneDayPlus.IsChecked ? ( diff.Value.Days + 1 ) : diff.Value.Days;
                }
            }

        }

        private void TotalDayWork( object? sender )
        {
            var Month_Work = sender as BindingList<Month_Work>;
            int day = 0;
            foreach ( var month in Month_Work )
            {
                day += month.dayofwork;
            }
            TotalDayWorkInYears = day;
        }

        private void OneDayPlus_Click( object sender, RoutedEventArgs e )
        {
            DayWorkInMonth( this.MW_ );
        }

        private void OneDayPlus_Initialized( object sender, EventArgs e )
        {
            var check = sender as CheckBox;
            if ( this.MW_[0]!.start.HasValue && this.MW_[0]!.stop.HasValue )
            {
                bool b = ( this.MW_[0]?.stop.Value.Day - this.MW_[0]?.start.Value.Day ) < this.MW_[0].dayofwork;
                if ( b )
                {
                    check.IsChecked = true;
                }
                else
                {
                    check.IsChecked = false;
                }
            }
            else
            {
                check.IsChecked = false;
            }
        }

        private void Show_total_txt_Loaded( object sender, RoutedEventArgs e )
        {
            TotalDayWork( this.MW_ );
            ShowTotalDay.Text = $"Общее количество отработанных дней в году: {TotalDayWorkInYears.ToString()}";
        }

        private void Page_Set_Work_Day_Closed( object sender, EventArgs e )
        {
            MainWindow.GetInstance.WorkDay.Text = TotalDayWorkInYears.ToString();
        }


    }
}
