using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using HotelBookingApp.Commands;
using HotelBookingApp.Models;
using HotelBookingApp.Services;
using HotelBookingApp.Views;

namespace HotelBookingApp.ViewModels
{
    public class HotelViewModel : INotifyPropertyChanged
    {
        private readonly BookingService _bookingService;
        private readonly AuthService _auth;
        private readonly NotificationService _notifications;

        private bool _isBusy;
        private string _statusMessage;
        private string _notificationMessage;
        private string _validationError;
        private RoomModel _selectedRoom;
        private BookingModel _selectedBooking;
        private string _guestName;
        private DateTime _checkIn = DateTime.Today;
        private DateTime _checkOut = DateTime.Today.AddDays(1);

        public ObservableCollection<RoomModel> Rooms { get; } = new ObservableCollection<RoomModel>();
        public ObservableCollection<BookingModel> Bookings { get; } = new ObservableCollection<BookingModel>();

        public ICommand BookCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand OpenChatCommand { get; }
        public ICommand LogoutCommand { get; }

        public bool IsBusy { get => _isBusy; set { _isBusy = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNotBusy)); } }
        public bool IsNotBusy => !_isBusy;
        public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }
        public string NotificationMessage { get => _notificationMessage; set { _notificationMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasNotification)); } }
        public bool HasNotification => !string.IsNullOrEmpty(NotificationMessage);
        public string ValidationError { get => _validationError; set { _validationError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); } }
        public bool HasError => !string.IsNullOrEmpty(ValidationError);
        public RoomModel SelectedRoom { get => _selectedRoom; set { _selectedRoom = value; OnPropertyChanged(); } }
        public BookingModel SelectedBooking { get => _selectedBooking; set { _selectedBooking = value; OnPropertyChanged(); } }
        public string GuestName { get => _guestName; set { _guestName = value; OnPropertyChanged(); } }
        public DateTime CheckIn { get => _checkIn; set { _checkIn = value; OnPropertyChanged(); } }
        public DateTime CheckOut { get => _checkOut; set { _checkOut = value; OnPropertyChanged(); } }

        public string CurrentUserDisplay => _auth.CurrentUser != null
            ? $"{_auth.CurrentUser.FullName} ({_auth.CurrentUser.Role})" : "";
        public bool IsManager => _auth.CurrentUser?.Role == "Manager";

        public HotelViewModel(BookingService bookingService, AuthService auth,
                               NotificationService notifications)
        {
            _bookingService = bookingService;
            _auth = auth;
            _notifications = notifications;

            BookCommand = new RelayCommand(async _ => await BookAsync(),
                _ => !IsBusy && SelectedRoom?.IsAvailable == true);
            CancelCommand = new RelayCommand(async _ => await CancelAsync(),
                _ => !IsBusy && SelectedBooking != null);
            OpenChatCommand = new RelayCommand(_ => OpenChat());
            LogoutCommand = new RelayCommand(_ => Logout());

            _notifications.NotificationReceived += msg =>
                Application.Current.Dispatcher.Invoke(() => NotificationMessage = msg);
            _notifications.StartListening();

            LoadData();
        }

        private void LoadData()
        {
            Rooms.Clear();
            foreach (var r in _bookingService.GetRooms()) Rooms.Add(r);
            Bookings.Clear();
            foreach (var b in _bookingService.GetBookings()) Bookings.Add(b);
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(GuestName))
            { ValidationError = "Введите ФИО гостя."; return false; }
            if (GuestName.Trim().Split(' ').Length < 2)
            { ValidationError = "Введите имя и фамилию (минимум два слова)."; return false; }
            if (CheckIn < DateTime.Today)
            { ValidationError = "Дата заезда не может быть в прошлом."; return false; }
            if (CheckOut <= CheckIn)
            { ValidationError = "Дата выезда должна быть позже даты заезда."; return false; }
            if (SelectedRoom == null)
            { ValidationError = "Выберите номер из списка."; return false; }
            ValidationError = null;
            return true;
        }

        private async System.Threading.Tasks.Task BookAsync()
        {
            if (!Validate()) return;
            IsBusy = true;
            StatusMessage = $"⏳ Подтверждение бронирования номера {SelectedRoom.RoomNumber}...";
            try
            {
                var username = _auth.CurrentUser?.Username ?? "guest";
                var booking = await _bookingService.BookRoomAsync(
                    SelectedRoom, GuestName.Trim(), username, CheckIn, CheckOut);
                Application.Current.Dispatcher.Invoke(() => Bookings.Add(booking));
                _notifications.Publish($"🛎 Новое бронирование: номер {booking.RoomNumber} для {booking.GuestName}");
                StatusMessage = $"✅ Номер {booking.RoomNumber} забронирован!";
                GuestName = string.Empty;
                ValidationError = null;
            }
            catch (Exception ex) { StatusMessage = $"❌ Ошибка: {ex.Message}"; }
            finally { IsBusy = false; }
        }

        private async System.Threading.Tasks.Task CancelAsync()
        {
            var booking = SelectedBooking;
            var room = Rooms.FirstOrDefault(r => r.RoomNumber == booking.RoomNumber);
            if (room == null) return;
            IsBusy = true;
            StatusMessage = $"⏳ Отмена брони #{booking.Id}...";
            try
            {
                await _bookingService.CancelBookingAsync(booking, room);
                Application.Current.Dispatcher.Invoke(() => Bookings.Remove(booking));
                _notifications.Publish($"🗑 Бронирование #{booking.Id} отменено. Номер {room.RoomNumber} свободен.");
                StatusMessage = $"🗑 Бронирование #{booking.Id} отменено.";
                SelectedBooking = null;
            }
            catch (Exception ex) { StatusMessage = $"❌ Ошибка: {ex.Message}"; }
            finally { IsBusy = false; }
        }

        private void OpenChat()
        {
            bool isManager = _auth.CurrentUser?.Role == "Manager";
            var chatService = new ChatService(_auth.CurrentUser?.Username ?? "Гость", isManager);
            var chatVm = new ChatViewModel(chatService);
            var chatWin = new ChatWindow { DataContext = chatVm };
            if (isManager) _ = chatService.StartServerAsync();
            else _ = chatService.ConnectAsync();
            chatWin.Show();
        }

        private void Logout()
        {
            _auth.Logout();
            var loginVm = new LoginViewModel(_auth);
            var loginWin = new LoginWindow { DataContext = loginVm };
            if (loginWin.ShowDialog() == true && loginVm.LoginSuccessful)
            {
                var mainVm = new HotelViewModel(_bookingService, _auth, _notifications);
                var mainWin = new MainWindow { DataContext = mainVm };
                mainWin.Show();
            }
            foreach (Window w in Application.Current.Windows)
                if (w is MainWindow && w.DataContext == this) { w.Close(); break; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}