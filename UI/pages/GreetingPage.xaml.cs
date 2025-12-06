using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
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
using UI.classes;

namespace UI.pages
{
    /// <summary>
    /// Interaction logic for GreetingPage.xaml
    /// </summary>
    public partial class GreetingPage : Page
    {
        private Dictionary<DayPart, string> _greetingString = new Dictionary<DayPart, string>()
        {
            { DayPart.Morning, "Доброе утро!" },
            { DayPart.Day, "Добрый день!" },
            { DayPart.Evening, "Добрый вечер!" },
            { DayPart.Night, "Доброй ночи!" }
        };

        public GreetingPage()
        {
            InitializeComponent();
            Loaded += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            txtblGreeting.Text = _greetingString[TimeManager.GetDayPart()];
        }

        private void GoAuthorisationPage(object sender, RoutedEventArgs e)
        {
            if (TimeManager.IsWorkHour() == false)
            {
                txtblError.Visibility = Visibility.Visible;
                return;
            }

            NavigationService.Navigate(new AuthorisationPage());
            //NavigationService.Navigate(new AdminPanel());
        }
    }
}
