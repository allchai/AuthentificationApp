using System.Windows.Controls;
using UI.classes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System;


namespace UI.pages
{
    enum CaptchaResult
    {
        Success, Failure
    };

    public partial class AuthorisationPage : Page
    {
        private Captcha _captcha;
        private int _captchaLength = 10;
        private bool _passedCaptcha;
        private int _enterAttempts = 0;

        private DispatcherTimer _banTimer;
        private int _banTimeRemains;        

        public AuthorisationPage()
        {
            InitializeComponent();
            Loaded += OnLoad;
            _captcha = new Captcha();
            ShowCaptcha(_captcha.Generate(_captchaLength));
        }

        public void OnLoad(object sender, EventArgs e)
        {
            _captcha = new Captcha();
            _banTimer = new DispatcherTimer();
            _banTimeRemains = 30;

            ShowCaptcha(_captcha.Generate(_captchaLength));

            _banTimer.Tick += new EventHandler(BanTimerTick);
            _banTimer.Interval = TimeSpan.FromSeconds(1);
        }

        private void ShowCaptcha(string captchaString)
        {
            txtbCaptcha.Text = captchaString;
            txtbCaptcha.Visibility = Visibility.Visible;
        }

        private void CheckCaptcha(object sender, RoutedEventArgs e)
        {
            if (_passedCaptcha) return;

            string input = txtbxCaptcha.Text;
            if (_captcha.CheckCaptcha(input) == true)
            {
                txtbCaptcha.Foreground = Brushes.Green;
                _passedCaptcha = true;
                ShowCaptchaResult(CaptchaResult.Success);
            } else
            {
                txtbxCaptcha.Text = string.Empty;
                ShowCaptcha(_captcha.Generate(_captchaLength));
                ShowCaptchaResult(CaptchaResult.Failure);
            }
        }

        private void ShowCaptchaResult(CaptchaResult result)
        {
            if (result == CaptchaResult.Success)
            {
                txtblCaptchaResult.Foreground = Brushes.Green;
                txtblCaptchaResult.Text = "Каптча успешно пройдена!";
            } else
            {
                txtblCaptchaResult.Foreground = Brushes.Red;
                txtblCaptchaResult.Text = "Каптча введена неверно!\nПопробуйте ещё раз!";
            }

            txtblCaptchaResult.Visibility = Visibility.Visible;
        }

        private void BanTimerTick(object sender, EventArgs e)
        {
            ShowError($"Количество ошибок превышено!\nОжидайте: {_banTimeRemains} секунд.");
            _banTimeRemains--;
            if (_banTimeRemains == 0)
            {
                btnEnter.IsEnabled = true;
                _banTimeRemains = 30;
                _enterAttempts = 0;
            }
        }

        private void ShowError(string text)
        {
            txtblError.Text = text;
            txtblError.Visibility = Visibility.Visible;
        }

        private void TryEnter(object sender, RoutedEventArgs e)
        {
            if (txtblLogin.Text == string.Empty ||
                txtblPassword.Text == string.Empty)
            {
                ShowError("Заполните все поля!");
                return;
            }

            if (_passedCaptcha == false)
            {
                ShowError("Пройдите каптчу!");
                return;
            }

            string login = txtblLogin.Text;
            string password = txtblPassword.Text;

            var db = new DataBase();
            User user = db.FindUser(login);
            
            if (user == null)
            {
                ShowError("Пользователя с таким\nименем не существует!");
                return;
            }
            bool enterResult = db.TryAuthorise(user, password);
            
            if (enterResult == false)
            {
                ShowError("Неправильный пароль!");
                _enterAttempts++;

                if (_enterAttempts >= 3)
                {
                    btnEnter.IsEnabled = false;
                    _banTimer.Start();
                }

                return;
            }
            NavigationService.Navigate(new UserPage(user));
        }

        private void GuestEnter(object sender, RoutedEventArgs e)
        {
            if (_passedCaptcha == false)
            {
                ShowError("Пройдите каптчу!");
                return;
            }
            User guestUser = new User()
            {
                Name = "Гость",
                Login = "Guest",
                Occupation = new Occupation()
                {
                    Name = "Guest"
                }
            };
            NavigationService.Navigate(guestUser);
        }
    }
}
