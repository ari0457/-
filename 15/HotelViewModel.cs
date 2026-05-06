using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace HotelBookingApp
{
    public class HotelViewModel : INotifyPropertyChanged
    {
        private readonly BookingService _bookingService = new BookingService();

        private bool _isBusy;
        private string _statusMessage;
        private RoomModel _selectedRoom;
        private BookingModel _selectedBooking;
        private string _guestName;
        private DateTime _checkIn = DateTime.Today;
        private DateTime _checkOut = DateTime.Today.AddDays(1);
        private string _validationError;

        public ObservableCollection<RoomModel> Rooms { get; } = new ObservableCollection<RoomModel>();
        public ObservableCollection<BookingModel> Bookings { get; } = new ObservableCollection<BookingModel>();

        public ICommand BookCommand { get; }
        public ICommand CancelCommand { get; }

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
            set { _selectedRoom = value; OnPropertyChanged(); }
        }

        public BookingModel SelectedBooking
        {
            get => _selectedBooking;
            set { _selectedBooking = value; OnPropertyChanged(); }
        }

        public string GuestName
        {
            get => _guestName;
            set { _guestName = value; OnPropertyChanged(); }
        }

        public DateTime CheckIn
        {
            get => _checkIn;
            set { _checkIn = value; OnPropertyChanged(); }
        }

        public DateTime CheckOut
        {
            get => _checkOut;
            set { _checkOut = value; OnPropertyChanged(); }
        }

        public HotelViewModel()
        {
            BookCommand = new RelayCommand(
                async _ => await BookAsync(),
                _ => !IsBusy && SelectedRoom?.IsAvailable == true);

            CancelCommand = new RelayCommand(
                async _ => await CancelAsync(),
                _ => !IsBusy && SelectedBooking != null);

            LoadRooms();
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(GuestName))
            {
                ValidationError = "Введите ФИО гостя.";
                return false;
            }
            if (GuestName.Trim().Split(' ').Length < 2)
            {
                ValidationError = "Введите имя и фамилию (минимум два слова).";
                return false;
            }
            if (CheckIn < DateTime.Today)
            {
                ValidationError = "Дата заезда не может быть в прошлом.";
                return false;
            }
            if (CheckOut <= CheckIn)
            {
                ValidationError = "Дата выезда должна быть позже даты заезда.";
                return false;
            }
            if (SelectedRoom == null)
            {
                ValidationError = "Выберите номер из списка.";
                return false;
            }

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
                var booking = await _bookingService.BookRoomAsync(
                    SelectedRoom, GuestName.Trim(), CheckIn, CheckOut);

                Application.Current.Dispatcher.Invoke(() => Bookings.Add(booking));

                StatusMessage = $"✅ Номер {booking.RoomNumber} забронирован для «{booking.GuestName}»!";
                GuestName = string.Empty;
                ValidationError = null;
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Ошибка: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
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
                StatusMessage = $"🗑 Бронирование #{booking.Id} отменено. Номер {room.RoomNumber} снова свободен.";
                SelectedBooking = null;
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Ошибка: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void LoadRooms()
        {
            Rooms.Add(new RoomModel { RoomNumber = 101, RoomType = "Стандарт", PricePerNight = 3500, Description = "Двуспальная кровать, Wi-Fi, TV", IsAvailable = true });
            Rooms.Add(new RoomModel { RoomNumber = 102, RoomType = "Стандарт", PricePerNight = 3500, Description = "Две кровати, Wi-Fi, TV", IsAvailable = true });
            Rooms.Add(new RoomModel { RoomNumber = 201, RoomType = "Люкс", PricePerNight = 7000, Description = "Джакузи, панорамный вид, мини-бар", IsAvailable = true });
            Rooms.Add(new RoomModel { RoomNumber = 202, RoomType = "Люкс", PricePerNight = 7500, Description = "Отдельная гостиная, балкон", IsAvailable = true });
            Rooms.Add(new RoomModel { RoomNumber = 301, RoomType = "Президентский", PricePerNight = 18000, Description = "2 спальни, кухня, личный дворецкий", IsAvailable = true });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}