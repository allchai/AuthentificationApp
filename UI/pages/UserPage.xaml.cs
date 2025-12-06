using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UI.classes;

namespace UI.pages
{
    public partial class UserPage : Page
    {
        private User _user;
        public UserPage(User user)
        {
            InitializeComponent();
            _user = user;
            Loaded += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            txtblGreeting.Text = $"Добро пожаловать, {_user.Name}";
            txtblLogin.Text = _user.Login;
            txtblOccupation.Text = _user.Occupation.Name;
            if (_user.Occupation.Name == "Менеджер")
                btnEnterAdminPanel.Visibility = Visibility.Visible;
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorisationPage());
        }

        private void EnterAdminPanel(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AdminPanel());
        }
    }
}
