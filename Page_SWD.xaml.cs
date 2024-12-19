using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
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
                    //IsReadOnly = false,
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
                    //IsReadOnly = false,
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

        private List<Month_Work> MW_ = new List<Month_Work>();

        private struct Month_Work
        {
            public string month { get; }
            public DateTime? start { get; set; }
            public DateTime? stop { get; set; }
            public int dayofwork { get; }

            public Month_Work(string txt)
            {
                month = txt;

            }

        }

        public Page_SWD()
        {
            InitializeComponent();

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        private void My_DataGrid_Init(object sender, EventArgs e)
        {
            Month_Work_Set();
            DataGrid_Set();
            

        }

        private void Month_Work_Set()
        {
            DateTime nowMonth = DateTime.Now;

            for (int i = 1; i <= nowMonth.Month; i++)
            {
                MW_.Add(new Month_Work(nowMonth.ToString("MMMM")));
            }
            My_DataGrid.ItemsSource = MW_;
        }
    }
}
