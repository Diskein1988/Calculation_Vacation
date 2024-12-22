using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
                    FontSize = 12,
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
                    FontSize = 12,
                    IsReadOnly = false,
                    Binding = new Binding("start")
                    
          },
          new DataGridTextColumn()
          {
                    Header = "День выезда с вахты",
                    Width = DataGridLength.SizeToHeader,
                    CanUserReorder = false,
                    CanUserResize = false,
                    CanUserSort = false,
                    FontSize = 12,
                    IsReadOnly = false,
                    Binding = new Binding("stop")
          },
          new DataGridTextColumn()
          {
                    Header = "Кол-во отработаных дней в месяце",
                    Width = DataGridLength.SizeToHeader,
                    CanUserReorder = false,
                    CanUserResize = false,
                    CanUserSort = false,
                    FontSize = 12,
                    IsReadOnly = true,
                    Binding = new Binding("dayofwork")
          },
        };

        private BindingList<Month_Work> MW_;

        public class Month_Work : INotifyPropertyChanged
        {
            private DateTime _start;
            private DateTime? _stop;

            public string month { get; set; }

            public DateTime start
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
                    OnpropertiChang( "start" );
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
                    if( _stop == value)
                    {
                        return;
                    }
                    _stop = value;
                    OnpropertiChang( "stop" );
                }
            }

            public int dayofwork { get; }

            public Month_Work( string txt )
            {
                month = txt;
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            private void OnpropertiChang(string txt = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs (txt));
            }
        }

        public Page_SWD()
        {
            _dataSaver = new DataSaver();
            InitializeComponent();
           
        }

        private void DataGrid_Set()
        {
            for ( int i = 0; i < colum.Count; i++ )
            {
                My_DataGrid.Columns.Add( colum[i] );
            }
        }

        public void DataGrid_Set_Wieth()
        {
            double wirth_1 = 0;
            for ( int i = 0; i < this.My_DataGrid.Columns.Count; i++ )
            {
                wirth_1 += this.My_DataGrid.Columns[i].ActualWidth;
            }
            this.My_DataGrid.Width = wirth_1 + 15;
        }

        private void My_DataGrid_Loaded( object sender, RoutedEventArgs e )
        {
            DataGrid_Set_Wieth();            

        }

        private void MW__ListChanged( object? sender, ListChangedEventArgs e )
        {
            if (e.ListChangedType == ListChangedType.ItemChanged )
            {
                Txt_test.Text = "sdsdsa";
            }
        }

        private void My_DataGrid_Init( object sender, EventArgs e )
        {
            
            Month_Work_Set();
            DataGrid_Set();
            MW_.ListChanged += MW__ListChanged;
        }

        private void Month_Work_Set()
        {
            if ( _dataSaver.TableIsCreate )
            {
                MW_ = _dataSaver.LoadDate();                
                My_DataGrid.ItemsSource = MW_;
            }
            else
            {
                DateTime nowMonth = DateTime.Now;
                MW_ = new BindingList<Month_Work>();

                for ( int i = 1; i <= nowMonth.Month; i++ )
                {
                    MW_.Add( new Month_Work( nowMonth.ToString( "MMMM" ) ) );
                }
                My_DataGrid.ItemsSource = MW_;
            }
            }

        private void My_DataGrid_BeginningEdit( object sender, DataGridBeginningEditEventArgs e )
        {
            
        }

        private void My_DataGrid_CellEditEnding( object sender, DataGridCellEditEndingEventArgs e )
        {
            

        }

        private void My_DataGrid_RowEditEnding( object sender, DataGridRowEditEndingEventArgs e )
        {
            var ee = e.Row.Item as Month_Work;
            ee.start = DateTime.Parse ("06.06.2666");
            


        }

        private void Button_Click( object sender, RoutedEventArgs e )
        {
            Txt_test.Text = MW_[0].start.Date.ToShortDateString();
            _dataSaver.SaveDate( MW_ );
        }
    }
}
