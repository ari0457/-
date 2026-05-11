using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using HotelBookingApp.Models;

namespace HotelBookingApp.Services
{
    public class HotelData
    {
        public List<RoomModel> Rooms { get; set; } = new List<RoomModel>();
        public List<BookingModel> Bookings { get; set; } = new List<BookingModel>();
    }

    public class BookingService
    {
        private readonly string _dataFile = "hotel_data.json";
        private HotelData _data = new HotelData();
        private int _nextId = 1;

        public BookingService()
        {
            Load();
            if (_data.Rooms.Count == 0)
                InitDefaultRooms();
        }

        public List<RoomModel> GetRooms() => _data.Rooms;
        public List<BookingModel> GetBookings() => _data.Bookings;

        public async Task<BookingModel> BookRoomAsync(RoomModel room, string guestName,
            string guestUsername, DateTime checkIn, DateTime checkOut)
        {
            if (!room.IsAvailable)
                throw new InvalidOperationException("Номер уже занят.");

            await Task.Delay(3000);

            var booking = new BookingModel
            {
                Id = _nextId++,
                RoomNumber = room.RoomNumber,
                GuestName = guestName,
                GuestUsername = guestUsername,
                CheckIn = checkIn,
                CheckOut = checkOut,
                TotalPrice = room.PricePerNight * (checkOut - checkIn).Days
            };

            room.IsAvailable = false;
            _data.Bookings.Add(booking);
            Save();
            return booking;
        }

        public async Task CancelBookingAsync(BookingModel booking, RoomModel room)
        {
            await Task.Delay(2000);
            room.IsAvailable = true;
            _data.Bookings.Remove(booking);
            Save();
        }

        private void Load()
        {
            if (!File.Exists(_dataFile)) return;
            var json = File.ReadAllText(_dataFile);
            _data = JsonConvert.DeserializeObject<HotelData>(json) ?? new HotelData();
            if (_data.Bookings.Count > 0)
                _nextId = _data.Bookings[_data.Bookings.Count - 1].Id + 1;
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(_data, Formatting.Indented);
            File.WriteAllText(_dataFile, json);
        }

        private void InitDefaultRooms()
        {
            _data.Rooms.AddRange(new[]
            {
                new RoomModel { RoomNumber=101, RoomType="Стандарт",      PricePerNight=3500,  Description="Двуспальная кровать, Wi-Fi, TV",    IsAvailable=true },
                new RoomModel { RoomNumber=102, RoomType="Стандарт",      PricePerNight=3500,  Description="Две кровати, Wi-Fi, TV",             IsAvailable=true },
                new RoomModel { RoomNumber=201, RoomType="Люкс",          PricePerNight=7000,  Description="Джакузи, панорамный вид, мини-бар",  IsAvailable=true },
                new RoomModel { RoomNumber=202, RoomType="Люкс",          PricePerNight=7500,  Description="Отдельная гостиная, балкон",         IsAvailable=true },
                new RoomModel { RoomNumber=301, RoomType="Президентский", PricePerNight=18000, Description="2 спальни, кухня, личный дворецкий", IsAvailable=true },
            });
            Save();
        }
    }
}