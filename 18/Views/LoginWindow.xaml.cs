using System.Windows;
using HotelBooking.Data;
using HotelBooking.Services;

namespace HotelBooking.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService;
        private bool _isRegistering = false;

        public LoginWindow()
        {
            InitializeComponent();
            var ctx = new HotelDbContext();
            _authService = new AuthService(ctx);
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorLabel.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(UsernameBox.Text) || PasswordBox.SecurePassword.Length == 0)
            {
                ShowError("Заполните все поля");
                return;
            }

            if (_isRegistering)
            {
                if (string.IsNullOrWhiteSpace(FullNameBox.Text))
                {
                    ShowError("Введите полное имя");
                    return;
                }
                if (_authService.Register(UsernameBox.Text, PasswordBox.Password, FullNameBox.Text))
                {
                    MessageBox.Show("Регистрация успешна! Теперь войдите.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    ToggleMode(false);
                }
                else
                {
                    ShowError("Пользователь с таким именем уже существует");
                }
            }
            else
            {
                if (_authService.Login(UsernameBox.Text, PasswordBox.Password))
                {
                    var mainWindow = new MainWindow(_authService);
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    ShowError("Неверное имя пользователя или пароль");
                }
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
            => ToggleMode(!_isRegistering);

        private void ToggleMode(bool registering)
        {
            _isRegistering = registering;
            FormTitle.Text = registering ? "Регистрация" : "Вход в систему";
            ActionButton.Content = registering ? "Зарегистрироваться" : "Войти";
            ToggleButton.Content = registering ? "Уже есть аккаунт? Войти" : "Нет аккаунта? Зарегистрироваться";
            FullNameLabel.Visibility = registering ? Visibility.Visible : Visibility.Collapsed;
            FullNameBox.Visibility = registering ? Visibility.Visible : Visibility.Collapsed;
            ErrorLabel.Visibility = Visibility.Collapsed;
        }

        private void ShowError(string msg)
        {
            ErrorLabel.Text = msg;
            ErrorLabel.Visibility = Visibility.Visible;
        }
    }
}
