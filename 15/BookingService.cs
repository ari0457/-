using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp
{
    public class BookingService
    {
        private readonly List<BookingModel> _bookings = new List<BookingModel>();
        private int _nextId = 1;

        public async Task<BookingModel> BookRoomAsync(RoomModel room, string guestName,
                                                      DateTime checkIn, DateTime checkOut)
        {
            if (!room.IsAvailable)
                throw new InvalidOperationException("Номер уже занят.");

            await Task.Delay(3000);

            var booking = new BookingModel
            {
                Id = _nextId++,
                RoomNumber = room.RoomNumber,
                GuestName = guestName,
                CheckIn = checkIn,
                CheckOut = checkOut,
                TotalPrice = room.PricePerNight * (checkOut - checkIn).Days
            };

            _bookings.Add(booking);
            room.IsAvailable = false;

            return booking;
        }

        public async Task CancelBookingAsync(BookingModel booking, RoomModel room)
        {
            await Task.Delay(2000);

            room.IsAvailable = true;
            _bookings.Remove(booking);
        }
    }
}