using System;

namespace HotelBookingApp
{
    public class BookingModel
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public string GuestName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }

        public int Nights => (CheckOut - CheckIn).Days;

        public override string ToString()
            => $"Бронь #{Id} — Номер {RoomNumber}, Гость: {GuestName}, " +
               $"{CheckIn:dd.MM} – {CheckOut:dd.MM} ({Nights} ночей)";
    }
}