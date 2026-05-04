using HotelBookingApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace HotelBookingApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Room> rooms;
        private ObservableCollection<Booking> bookings;
        private Room selectedRoom;
        private Booking selectedBooking;
        private int nextBookingId = 1;

        public ICommand BookRoomCommand { get; }
        public ICommand EditBookingCommand { get; }
        public ICommand CancelBookingCommand { get; }

        public ObservableCollection<Room> Rooms
        {
            get => rooms;
            set { rooms = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Booking> Bookings
        {
            get => bookings;
            set { bookings = value; OnPropertyChanged(); }
        }

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

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Rooms = new ObservableCollection<Room>
            {
                new Room(101, "Стандарт",  100),
                new Room(102, "Стандарт",  100),
                new Room(201, "Люкс",      350),
                new Room(202, "Люкс",      350),
                new Room(301, "Семейный",  200),
                new Room(302, "Семейный",  200)
            };

            Bookings = new ObservableCollection<Booking>();

            RoomsList.ItemsSource = Rooms;
            BookingsList.ItemsSource = Bookings;

            BookRoomCommand = new RelayCommand(_ => BookRoom());
            EditBookingCommand = new RelayCommand(_ => EditBooking(), _ => SelectedBooking != null);
            CancelBookingCommand = new RelayCommand(_ => CancelBooking(), _ => SelectedBooking != null);
        }

        private void BookRoom()
        {
            var available = Rooms.Where(r => r.IsAvailable).ToList();
            if (!available.Any())
            {
                MessageBox.Show("Нет свободных номеров!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var win = new BookingWindow(available) { Owner = this };
            if (win.ShowDialog() == true)
            {
                var b = new Booking
                {
                    Id = nextBookingId++,
                    GuestName = win.GuestName,
                    RoomNumber = win.SelectedRoom.Number,
                    StartDate = win.StartDate,
                    EndDate = win.EndDate,
                    TotalPrice = win.TotalPrice
                };
                Bookings.Add(b);
                Rooms.First(r => r.Number == b.RoomNumber).IsAvailable = false;
                Refresh();
                StatusText.Text = $"Забронирован №{b.RoomNumber} для {b.GuestName}";
            }
        }

        private void EditBooking()
        {
            if (SelectedBooking == null) return;

            Rooms.First(r => r.Number == SelectedBooking.RoomNumber).IsAvailable = true;

            var available = Rooms.Where(r => r.IsAvailable).ToList();
            var win = new BookingWindow(available, SelectedBooking) { Owner = this };

            if (win.ShowDialog() == true)
            {
                SelectedBooking.GuestName = win.GuestName;
                SelectedBooking.RoomNumber = win.SelectedRoom.Number;
                SelectedBooking.StartDate = win.StartDate;
                SelectedBooking.EndDate = win.EndDate;
                SelectedBooking.TotalPrice = win.TotalPrice;
                Rooms.First(r => r.Number == SelectedBooking.RoomNumber).IsAvailable = false;
                StatusText.Text = $"Бронь #{SelectedBooking.Id} обновлена";
            }
            else
            {
                Rooms.First(r => r.Number == SelectedBooking.RoomNumber).IsAvailable = false;
            }

            Refresh();
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
                Refresh();
                StatusText.Text = "Бронь отменена";
            }
        }

        private void Refresh()
        {
            var r = Rooms.ToList();
            Rooms.Clear();
            foreach (var x in r) Rooms.Add(x);

            var b = Bookings.ToList();
            Bookings.Clear();
            foreach (var x in b) Bookings.Add(x);
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Close();

        private void About_Click(object sender, RoutedEventArgs e) =>
            MessageBox.Show("Система бронирования отеля\nВерсия 1.0",
                "О программе", MessageBoxButton.OK, MessageBoxImage.Information);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}