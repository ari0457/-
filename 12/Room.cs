using System;

namespace HotelBookingApp
{
    public class Room
    {
        public int Number { get; set; }
        public string Type { get; set; }
        public int PricePerNight { get; set; }

        public Room(int number, string type, int pricePerNight)
        {
            Number = number;
            Type = type;
            PricePerNight = pricePerNight;
        }
    }
}