using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Error.xaml
    /// </summary>
    public partial class Error : Window
    {
        public Error()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Error_Win.Close();
        }

        public void Error_Show(string txt)
        {
            switch (txt)
            {
                case "Err_Null":
                    Err_reason_txt.Text = "Не введено количество отработаных дней в году";
                    Error_Win.Show();
                    break;
                case "Err_NoDigi":
                    Err_reason_txt.Text = "Нужно ввести только цифры";
                    Error_Win.Show();
                    break;
                case "Err_more_day":
                    Err_reason_txt.Text = "Введено число больше чем дней в году";
                    Error_Win.Show();
                    break;
                default:
                    break;
            }
        }
    }
}
