using HotelBookingApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace HotelBookingApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Booking selectedBooking;
        private Room selectedRoom;
        private int nextId = 1;

        public ObservableCollection<Room> Rooms { get; } = new ObservableCollection<Room>();
        public ObservableCollection<Booking> Bookings { get; } = new ObservableCollection<Booking>();

        public Room SelectedRoom
        {
            get => selectedRoom;
            set { selectedRoom = value; OnPropertyChanged(); }
        }

        public Booking SelectedBooking
        {
            get => selectedBooking;
            set { selectedBooking = value; OnPropertyChanged(); }
        }

        public ICommand BookRoomCommand { get; }
        public ICommand EditBookingCommand { get; }
        public ICommand CancelBookingCommand { get; }

        public MainViewModel()
        {
            Rooms.Add(new Room(101, "Стандарт", 2500));
            Rooms.Add(new Room(102, "Стандарт", 2500));
            Rooms.Add(new Room(201, "Люкс", 4500));
            Rooms.Add(new Room(202, "Люкс", 4500));
            Rooms.Add(new Room(301, "Семейный", 3500));
            Rooms.Add(new Room(302, "Семейный", 3500));

            BookRoomCommand = new RelayCommand(_ => BookRoom());
            EditBookingCommand = new RelayCommand(_ => EditBooking(), _ => SelectedBooking != null);
            CancelBookingCommand = new RelayCommand(_ => CancelBooking(), _ => SelectedBooking != null);
        }

        private void BookRoom()
        {
            var available = Rooms.Where(r => r.IsAvailable).ToList();
            if (!available.Any())
            {
                MessageBox.Show("Нет свободных номеров!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var win = new BookingWindow(available);
            if (win.ShowDialog() == true)
            {
                var b = new Booking
                {
                    Id = nextId++,
                    GuestName = win.GuestName,
                    GuestPhone = win.GuestPhone,
                    RoomNumber = win.SelectedRoom.Number,
                    StartDate = win.StartDate,
                    EndDate = win.EndDate,
                    TotalPrice = win.TotalPrice
                };
                Bookings.Add(b);
                Rooms.First(r => r.Number == b.RoomNumber).IsAvailable = false;
            }
        }

        private void EditBooking()
        {
            if (SelectedBooking == null) return;

            Rooms.First(r => r.Number == SelectedBooking.RoomNumber).IsAvailable = true;
            var available = Rooms.Where(r => r.IsAvailable).ToList();

            var win = new BookingWindow(available, SelectedBooking);
            if (win.ShowDialog() == true)
            {
                SelectedBooking.GuestName = win.GuestName;
                SelectedBooking.GuestPhone = win.GuestPhone;
                SelectedBooking.RoomNumber = win.SelectedRoom.Number;
                SelectedBooking.StartDate = win.StartDate;
                SelectedBooking.EndDate = win.EndDate;
                SelectedBooking.TotalPrice = win.TotalPrice;
                Rooms.First(r => r.Number == SelectedBooking.RoomNumber).IsAvailable = false;
            }
            else
            {
                Rooms.First(r => r.Number == SelectedBooking.RoomNumber).IsAvailable = false;
            }
        }

        private void CancelBooking()
        {
            if (SelectedBooking == null) return;

            var res = MessageBox.Show(
                $"Отменить бронь для {SelectedBooking.GuestName} (№{SelectedBooking.RoomNumber})?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (res == MessageBoxResult.Yes)
            {
                Rooms.First(r => r.Number == SelectedBooking.RoomNumber).IsAvailable = true;
                Bookings.Remove(SelectedBooking);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}