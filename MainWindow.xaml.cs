using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculation_Vacation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calendar calendar = new Calendar();
        Page_SWD page_SWD;
        Error error;

        public MainWindow()
        {
            InitializeComponent();
            gone_days.Text = calendar.Dayofyers;
        }

        private void Button_Click_Calculate(object sender, RoutedEventArgs e)
        {
            Day_OSN.Text = calendar.Day_osn();
            DAY_DOP.Text = calendar.Day_dop(WorkDay.Text);
            error = new Error();
            error.Error_Show(DAY_DOP.Text);
        }

        private void Button_Click_Set_Work_Day(object sender, RoutedEventArgs e)
        {
            page_SWD = new();
            page_SWD.Show();
            WorkDay.Text = page_SWD.TotalDayWorkInYers.ToString();
        }

        private void MainWin_Closed(object sender, EventArgs e)
        {
            page_SWD.Close();
            error.Close();
        }
    }
}