using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HotelBookingApp.Models
{
    public class Booking : INotifyPropertyChanged
    {
        private string guestName;
        private string guestPhone;
        private int roomNumber;
        private DateTime startDate;
        private DateTime endDate;
        private int totalPrice;

        public int Id { get; set; }

        public string GuestName
        {
            get => guestName;
            set { guestName = value; OnPropertyChanged(); }
        }

        public string GuestPhone
        {
            get => guestPhone;
            set { guestPhone = value; OnPropertyChanged(); }
        }

        public int RoomNumber
        {
            get => roomNumber;
            set { roomNumber = value; OnPropertyChanged(); }
        }

        public DateTime StartDate
        {
            get => startDate;
            set { startDate = value; OnPropertyChanged(); OnPropertyChanged(nameof(DateRange)); }
        }

        public DateTime EndDate
        {
            get => endDate;
            set { endDate = value; OnPropertyChanged(); OnPropertyChanged(nameof(DateRange)); }
        }

        public int TotalPrice
        {
            get => totalPrice;
            set { totalPrice = value; OnPropertyChanged(); }
        }

        public string DateRange => $"{StartDate:dd.MM.yy} – {EndDate:dd.MM.yy}";

        public override string ToString() =>
            $"#{Id} | {GuestName} | №{RoomNumber} | {DateRange} | {TotalPrice} руб.";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}