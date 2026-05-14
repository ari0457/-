using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.ViewModels;

namespace HotelBooking.Views
{
    public partial class BookingDialog : Window
    {
        public BookingDialogViewModel DialogViewModel { get; }
        private readonly HotelDbContext _ctx;

        public BookingDialog(HotelDbContext ctx, Booking? existingBooking = null)
        {
            InitializeComponent();
            _ctx = ctx;
            DialogViewModel = new BookingDialogViewModel();

            var availableRooms = ctx.Rooms.Where(r => r.IsAvailable).ToList();

            if (existingBooking != null)
            {
                // Edit mode — include the currently booked room
                DialogTitle.Text = "✏️ Редактирование бронирования";
                if (existingBooking.Room != null && !availableRooms.Contains(existingBooking.Room))
                    availableRooms.Insert(0, existingBooking.Room);

                DialogViewModel.CheckIn = existingBooking.CheckIn;
                DialogViewModel.CheckOut = existingBooking.CheckOut;
                if (existingBooking.Client != null)
                {
                    DialogViewModel.ClientFullName = existingBooking.Client.FullName;
                    DialogViewModel.ClientPhone = existingBooking.Client.Phone;
                    DialogViewModel.ClientEmail = existingBooking.Client.Email;
                }
            }

            foreach (var r in availableRooms)
                DialogViewModel.AvailableRooms.Add(r);

            RoomCombo.ItemsSource = DialogViewModel.AvailableRooms;

            if (existingBooking?.Room != null)
                DialogViewModel.SelectedRoom = DialogViewModel.AvailableRooms.FirstOrDefault(r => r.Id == existingBooking.Room.Id);
            else if (DialogViewModel.AvailableRooms.Count > 0)
                DialogViewModel.SelectedRoom = DialogViewModel.AvailableRooms[0];

            DataContext = DialogViewModel;
        }

        private void RoomCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DialogViewModel.SelectedRoom = RoomCombo.SelectedItem as Models.Room;
        }

        private void DatePicker_Changed(object? sender, SelectionChangedEventArgs e)
        {
            DialogViewModel.CheckIn = CheckInPicker.SelectedDate ?? System.DateTime.Today;
            DialogViewModel.CheckOut = CheckOutPicker.SelectedDate ?? System.DateTime.Today.AddDays(1);
            DialogViewModel.Validate();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            DialogViewModel.Validate();
            if (!DialogViewModel.IsValid)
            {
                return;
            }
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
