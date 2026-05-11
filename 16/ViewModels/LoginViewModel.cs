using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using HotelBookingApp.Commands;
using HotelBookingApp.Services;

namespace HotelBookingApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly AuthService _auth;
        private string _username;
        private string _password;
        private string _fullName;
        private string _errorMessage;
        private bool _isRegisterMode;

        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public string FullName { get => _fullName; set { _fullName = value; OnPropertyChanged(); } }
        public string ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); } }
        public bool IsRegisterMode { get => _isRegisterMode; set { _isRegisterMode = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsLoginMode)); } }
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public bool IsLoginMode => !_isRegisterMode;
        public bool LoginSuccessful { get; private set; }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand SwitchModeCommand { get; }

        public LoginViewModel(AuthService auth)
        {
            _auth = auth;
            LoginCommand = new RelayCommand(_ => DoLogin());
            RegisterCommand = new RelayCommand(_ => DoRegister());
            SwitchModeCommand = new RelayCommand(_ => { IsRegisterMode = !IsRegisterMode; ErrorMessage = null; });
        }

        private void DoLogin()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            { ErrorMessage = "Введите логин и пароль."; return; }

            if (!_auth.Login(Username, Password))
            { ErrorMessage = "Неверный логин или пароль."; return; }

            LoginSuccessful = true;
            CloseWindow();
        }

        private void DoRegister()
        {
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(FullName))
            { ErrorMessage = "Заполните все поля."; return; }

            if (!_auth.Register(Username, Password, FullName, "Guest"))
            { ErrorMessage = "Пользователь с таким логином уже существует."; return; }

            IsRegisterMode = false;
            ErrorMessage = "Регистрация успешна! Войдите в систему.";
        }

        private void CloseWindow()
        {
            foreach (Window w in Application.Current.Windows)
                if (w.DataContext == this) { w.DialogResult = true; w.Close(); break; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}