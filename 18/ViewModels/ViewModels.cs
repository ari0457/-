using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HotelBooking.Commands;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Repositories;
using HotelBooking.Services;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }
    }

    public class HotelViewModel : BaseViewModel
    {
        private readonly HotelDbContext _ctx;
        private readonly BookingService _bookingService;
        private readonly BookingRepository _bookingRepo;
        private readonly RoomRepository _roomRepo;

        private bool _isBusy;
        private string _busyMessage = "";
        private Room? _selectedRoom;
        private Booking? _selectedBooking;
        private string _statusMessage = "";

        public ObservableCollection<Room> Rooms { get; } = new();
        public ObservableCollection<Booking> Bookings { get; } = new();
        public ObservableCollection<Room> AvailableRooms { get; } = new();

        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }
        public string BusyMessage { get => _busyMessage; set => SetProperty(ref _busyMessage, value); }
        public string StatusMessage { get => _statusMessage; set => SetProperty(ref _statusMessage, value); }

        public Room? SelectedRoom
        {
            get => _selectedRoom;
            set { SetProperty(ref _selectedRoom, value); OnPropertyChanged(nameof(IsRoomSelected)); }
        }

        public Booking? SelectedBooking
        {
            get => _selectedBooking;
            set { SetProperty(ref _selectedBooking, value); OnPropertyChanged(nameof(IsBookingSelected)); }
        }

        public bool IsRoomSelected => _selectedRoom != null;
        public bool IsBookingSelected => _selectedBooking != null;

        public ICommand BookRoomCommand { get; }
        public ICommand EditBookingCommand { get; }
        public ICommand CancelBookingCommand { get; }
        public ICommand RefreshCommand { get; }

        public HotelViewModel(HotelDbContext ctx, BookingService bookingService)
        {
            _ctx = ctx;
            _bookingService = bookingService;
            _bookingRepo = new BookingRepository(ctx);
            _roomRepo = new RoomRepository(ctx);

            BookRoomCommand = new AsyncRelayCommand(ExecuteBookRoom);
            EditBookingCommand = new AsyncRelayCommand(ExecuteEditBooking, _ => IsBookingSelected);
            CancelBookingCommand = new AsyncRelayCommand(ExecuteCancelBooking, _ => IsBookingSelected);
            RefreshCommand = new AsyncRelayCommand(LoadDataAsync);

            _ = LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            IsBusy = true;
            BusyMessage = "Загрузка данных...";
            try
            {
                await Task.Delay(200);
                var rooms = await _roomRepo.GetAllAsync();
                var bookings = await _bookingRepo.GetAllAsync();

                Rooms.Clear();
                AvailableRooms.Clear();
                foreach (var r in rooms)
                {
                    Rooms.Add(r);
                    if (r.IsAvailable) AvailableRooms.Add(r);
                }

                Bookings.Clear();
                foreach (var b in bookings.OrderByDescending(x => x.CreatedAt))
                    Bookings.Add(b);

                StatusMessage = $"Загружено номеров: {rooms.Count}, бронирований: {bookings.Count}";
            }
            finally { IsBusy = false; }
        }

        private async Task ExecuteBookRoom(object? _)
        {
            var dialog = new Views.BookingDialog(_ctx);
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                IsBusy = true;
                BusyMessage = "Обработка бронирования...";
                try
                {
                    var vm = dialog.DialogViewModel;
                    var booking = await _bookingService.BookRoomAsync(
                        vm.SelectedRoom!.Id,
                        new Client { FullName = vm.ClientFullName, Phone = vm.ClientPhone, Email = vm.ClientEmail },
                        vm.CheckIn, vm.CheckOut);

                    await LoadDataAsync();
                    StatusMessage = $"✅ Бронирование оформлено! Номер {booking.Room?.Number}, сумма: {booking.TotalPrice:C}";
                    MessageBox.Show($"Бронирование успешно оформлено!\nНомер: {booking.Room?.Number}\nСумма: {booking.TotalPrice:C}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка бронирования: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally { IsBusy = false; }
            }
        }

        private async Task ExecuteEditBooking(object? _)
        {
            if (SelectedBooking == null) return;
            var dialog = new Views.BookingDialog(_ctx, SelectedBooking);
            dialog.Owner = Application.Current.MainWindow;
            if (dialog.ShowDialog() == true)
            {
                IsBusy = true;
                BusyMessage = "Сохранение изменений...";
                try
                {
                    var vm = dialog.DialogViewModel;
                    SelectedBooking.CheckIn = vm.CheckIn;
                    SelectedBooking.CheckOut = vm.CheckOut;
                    if (SelectedBooking.Client != null)
                    {
                        SelectedBooking.Client.FullName = vm.ClientFullName;
                        SelectedBooking.Client.Phone = vm.ClientPhone;
                        SelectedBooking.Client.Email = vm.ClientEmail;
                        _ctx.Clients.Update(SelectedBooking.Client);
                    }
                    await _bookingService.UpdateBookingAsync(SelectedBooking);
                    await LoadDataAsync();
                    StatusMessage = "✅ Бронирование обновлено";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally { IsBusy = false; }
            }
        }

        private async Task ExecuteCancelBooking(object? _)
        {
            if (SelectedBooking == null) return;
            var result = MessageBox.Show(
                $"Вы уверены, что хотите отменить бронирование?\nНомер: {SelectedBooking.Room?.Number}\nКлиент: {SelectedBooking.Client?.FullName}",
                "Подтверждение отмены",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            IsBusy = true;
            BusyMessage = "Отмена бронирования...";
            try
            {
                await _bookingService.CancelBookingAsync(SelectedBooking.Id);
                await LoadDataAsync();
                StatusMessage = "✅ Бронирование отменено";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally { IsBusy = false; }
        }
    }

    public class BookingDialogViewModel : BaseViewModel
    {
        private Room? _selectedRoom;
        private string _clientFullName = "";
        private string _clientPhone = "";
        private string _clientEmail = "";
        private DateTime _checkIn = DateTime.Today;
        private DateTime _checkOut = DateTime.Today.AddDays(1);
        private string _validationError = "";

        public ObservableCollection<Room> AvailableRooms { get; } = new();

        public Room? SelectedRoom { get => _selectedRoom; set { SetProperty(ref _selectedRoom, value); OnPropertyChanged(nameof(TotalPrice)); OnPropertyChanged(nameof(Nights)); } }
        public string ClientFullName { get => _clientFullName; set { SetProperty(ref _clientFullName, value); Validate(); } }
        public string ClientPhone { get => _clientPhone; set { SetProperty(ref _clientPhone, value); Validate(); } }
        public string ClientEmail { get => _clientEmail; set => SetProperty(ref _clientEmail, value); }
        public DateTime CheckIn { get => _checkIn; set { SetProperty(ref _checkIn, value); OnPropertyChanged(nameof(TotalPrice)); OnPropertyChanged(nameof(Nights)); Validate(); } }
        public DateTime CheckOut { get => _checkOut; set { SetProperty(ref _checkOut, value); OnPropertyChanged(nameof(TotalPrice)); OnPropertyChanged(nameof(Nights)); Validate(); } }
        public string ValidationError { get => _validationError; set => SetProperty(ref _validationError, value); }

        public int Nights => Math.Max(0, (CheckOut - CheckIn).Days);
        public decimal TotalPrice => SelectedRoom != null ? SelectedRoom.PricePerNight * Nights : 0;
        public bool IsValid => string.IsNullOrEmpty(ValidationError) && SelectedRoom != null && !string.IsNullOrWhiteSpace(ClientFullName) && !string.IsNullOrWhiteSpace(ClientPhone);

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ClientFullName))
                ValidationError = "Введите ФИО клиента";
            else if (string.IsNullOrWhiteSpace(ClientPhone))
                ValidationError = "Введите телефон клиента";
            else if (CheckIn < DateTime.Today)
                ValidationError = "Дата заезда не может быть в прошлом";
            else if (CheckOut <= CheckIn)
                ValidationError = "Дата выезда должна быть позже даты заезда";
            else
                ValidationError = "";

            OnPropertyChanged(nameof(IsValid));
        }
    }

    public class LoginViewModel : BaseViewModel
    {
        private string _username = "";
        private string _password = "";
        private string _errorMessage = "";
        private bool _isRegistering;

        public string Username { get => _username; set => SetProperty(ref _username, value); }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string ErrorMessage { get => _errorMessage; set => SetProperty(ref _errorMessage, value); }
        public bool IsRegistering { get => _isRegistering; set => SetProperty(ref _isRegistering, value); }
        public string FullName { get; set; } = "";
    }

    public class SettingsViewModel : BaseViewModel
    {
        private string _hotelName = "Grand Hotel";
        private string _hotelAddress = "ул. Советская, 1";
        private int _checkInTime = 14;
        private int _checkOutTime = 12;

        public string HotelName { get => _hotelName; set => SetProperty(ref _hotelName, value); }
        public string HotelAddress { get => _hotelAddress; set => SetProperty(ref _hotelAddress, value); }
        public int CheckInTime { get => _checkInTime; set => SetProperty(ref _checkInTime, value); }
        public int CheckOutTime { get => _checkOutTime; set => SetProperty(ref _checkOutTime, value); }
    }
}
