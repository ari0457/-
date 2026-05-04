using System;

namespace HotelBookingApp.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string GuestName { get; set; }
        public int RoomNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalPrice { get; set; }

        public override string ToString() =>
            $"#{Id} | {GuestName} | №{RoomNumber} | {StartDate:dd.MM.yy} – {EndDate:dd.MM.yy} | {TotalPrice} руб.";
    }
}