using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using HotelAnimatedApp.Commands;
using HotelAnimatedApp.Models;
using HotelAnimatedApp.Services;

namespace HotelAnimatedApp.ViewModels
{
    public class HotelViewModel : INotifyPropertyChanged
    {
        private readonly BookingService _bookingService = new BookingService();

        private bool _isBusy;
        private string _statusMessage;
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
        public ICommand SelectRoomCommand { get; }

        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNotBusy)); }
        }
        public bool IsNotBusy => !_isBusy;
        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }
        public string ValidationError
        {
            get => _validationError;
            set { _validationError = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasError)); }
        }
        public bool HasError => !string.IsNullOrEmpty(ValidationError);

        public RoomModel SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                // Сбросить выделение у предыдущего
                if (_selectedRoom != null) _selectedRoom.IsSelected = false;
                _selectedRoom = value;
                if (_selectedRoom != null) _selectedRoom.IsSelected = true;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedRoom));
            }
        }

        public bool HasSelectedRoom => _selectedRoom != null;

        public BookingModel SelectedBooking
        {
            get => _selectedBooking;
            set { _selectedBooking = value; OnPropertyChanged(); }
        }
        public string GuestName { get => _guestName; set { _guestName = value; OnPropertyChanged(); } }
        public DateTime CheckIn { get => _checkIn; set { _checkIn = value; OnPropertyChanged(); } }
        public DateTime CheckOut { get => _checkOut; set { _checkOut = value; OnPropertyChanged(); } }

        public HotelViewModel()
        {
            BookCommand = new RelayCommand(
                async _ => await BookAsync(),
                _ => !IsBusy && SelectedRoom?.IsAvailable == true);

            CancelCommand = new RelayCommand(
                async _ => await CancelAsync(),
                _ => !IsBusy && SelectedBooking != null);

            SelectRoomCommand = new RelayCommand(
                p => { if (p is RoomModel r) SelectedRoom = r; });

            LoadRooms();
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
            { ValidationError = "Выберите номер."; return false; }
            ValidationError = null;
            return true;
        }

        private async System.Threading.Tasks.Task BookAsync()
        {
            if (!Validate()) return;
            IsBusy = true;
            StatusMessage = $"⏳ Бронирование номера {SelectedRoom.RoomNumber}...";
            try
            {
                var booking = await _bookingService.BookRoomAsync(
                    SelectedRoom, GuestName.Trim(), CheckIn, CheckOut);
                Application.Current.Dispatcher.Invoke(() => Bookings.Add(booking));
                StatusMessage = $"✅ Номер {booking.RoomNumber} забронирован для «{booking.GuestName}»!";
                GuestName = string.Empty;
                ValidationError = null;
                SelectedRoom = null;
            }
            catch (Exception ex) { StatusMessage = $"❌ {ex.Message}"; }
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
                StatusMessage = $"🗑 Бронирование #{booking.Id} отменено.";
                SelectedBooking = null;
            }
            catch (Exception ex) { StatusMessage = $"❌ {ex.Message}"; }
            finally { IsBusy = false; }
        }

        private void LoadRooms()
        {
            Rooms.Add(new RoomModel { RoomNumber = 101, RoomType = "Стандарт", PricePerNight = 3500, Description = "Двуспальная кровать, Wi-Fi, TV", ImageIcon = "🛏", IsAvailable = true });
            Rooms.Add(new RoomModel { RoomNumber = 102, RoomType = "Стандарт", PricePerNight = 3500, Description = "Две кровати, Wi-Fi, TV", ImageIcon = "🛏", IsAvailable = true });
            Rooms.Add(new RoomModel { RoomNumber = 201, RoomType = "Люкс", PricePerNight = 7000, Description = "Джакузи, панорамный вид, мини-бар", ImageIcon = "✨", IsAvailable = true });
            Rooms.Add(new RoomModel { RoomNumber = 202, RoomType = "Люкс", PricePerNight = 7500, Description = "Отдельная гостиная, балкон", ImageIcon = "✨", IsAvailable = true });
            Rooms.Add(new RoomModel { RoomNumber = 301, RoomType = "Президентский", PricePerNight = 18000, Description = "2 спальни, кухня, личный дворецкий", ImageIcon = "👑", IsAvailable = true });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}