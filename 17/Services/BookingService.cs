using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelAnimatedApp.Models;

namespace HotelAnimatedApp.Services
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

            await Task.Delay(2000);

            var booking = new BookingModel
            {
                Id = _nextId++,
                RoomNumber = room.RoomNumber,
                GuestName = guestName,
                CheckIn = checkIn,
                CheckOut = checkOut,
                TotalPrice = room.PricePerNight * (checkOut - checkIn).Days
            };

            room.IsAvailable = false;
            _bookings.Add(booking);
            return booking;
        }

        public async Task CancelBookingAsync(BookingModel booking, RoomModel room)
        {
            await Task.Delay(1500);
            room.IsAvailable = true;
            _bookings.Remove(booking);
        }
    }
}