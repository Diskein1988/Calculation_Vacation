using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private static MainWindow instance;
        private Calendar calendar = new Calendar();
        private Page_SWD page_SWD;
        private Error error;
        public DateTime Start_work { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
            start_to.Text = calendar.BeginningOfYear;
            gone_days.Text = calendar.Dayofyers;
        }

        public static MainWindow GetInstance
        {
            get
            {
                return instance;
            }
        }

        private void Button_Click_Calculate( object sender, RoutedEventArgs e )
        {
            Day_OSN.Text = calendar.Day_osn();
            DAY_DOP.Text = calendar.Day_dop( WorkDay.Text );
            error = new Error();
            error.Error_Show( DAY_DOP.Text );
        }

        private void Button_Click_Set_Work_Day( object sender, RoutedEventArgs e )
        {
            page_SWD = new();
            page_SWD.Show();
            WorkDay.Text = page_SWD.TotalDayWorkInYears.ToString();
        }

        private void MainWin_Closed( object sender, EventArgs e )
        {
            page_SWD?.Close();
            error?.Close();
        }

        private void start_to_TextChanged( object sender, TextChangedEventArgs e )
        {
            var test = sender as TextBox;
            string pattern = @"^\d{2}\.\d{2}$"; //\.\d{4}

            if ( Regex.IsMatch( test.Text, pattern, RegexOptions.IgnoreCase ) )
            {
                if ( DateTime.TryParse( test.Text, out DateTime date ) )
                {
                    start_to.Text = date.ToString( "dd.MM" );
                    Start_work = date;
                    start_to.Background = new SolidColorBrush( Colors.LightGreen );
                    Set_Work_Day.IsEnabled = true;
                }
                else
                {
                    start_to.Background = new SolidColorBrush( Colors.OrangeRed );
                    Set_Work_Day.IsEnabled = false;
                }
            }
            else
            {
                start_to.Background = new SolidColorBrush( Colors.OrangeRed );
                Set_Work_Day.IsEnabled = false;
            }
        }

        private void Calculate_Go_MouseEnter( object sender, MouseEventArgs e )
        {
            Text_TextBox.Text = "наведено";
            var butt = sender as Button;
            butt.Background = new SolidColorBrush(Colors.LightGreen);
            Thread.Sleep( 1000 );
            ColorAnimation animation = new ColorAnimation
            {
                From = Colors.DarkGreen,
                To = Colors.Gold,
                Duration = new Duration( TimeSpan.FromSeconds( 2.5 ) ),
                AutoReverse = false,
               

            };
            VisualStateManager manager = new VisualStateManager();


            SolidColorBrush brush = new SolidColorBrush( Colors.Gold );
            animation.AccelerationRatio = 0.5;
            animation.FillBehavior = FillBehavior.Stop;
            butt.Background = brush;
            brush.BeginAnimation( SolidColorBrush.ColorProperty, animation );


        }

        private void But_Test_MouseEnter( object sender, MouseEventArgs e )
        {
            Text_TextBox.Text = "qwert";
            var butt = sender as Button;
            List<Storyboard> storyboards = new List<Storyboard>{  new Storyboard
            {
                Name = butt.Name,

            }};
            
            VisualStateGroup visualStateGroup = new VisualStateGroup
            {
                Name =  "123",
            };


            Thread.Sleep( 1000 );
            ColorAnimation animation = new ColorAnimation
            {
                From = Colors.DarkGreen,
                To = Colors.Gold,
                Duration = new Duration( TimeSpan.FromSeconds( 2.5 ) ),
                AutoReverse = false

            };

            SolidColorBrush brush = new SolidColorBrush( Colors.Gold );
            animation.AccelerationRatio = 0.5;
            animation.FillBehavior = FillBehavior.Stop;
            butt.Background = brush;
            brush.BeginAnimation( SolidColorBrush.ColorProperty, animation );

        }

        private void Set_Work_Day_Initialized(object sender, EventArgs e)
        {

        }
    }
}