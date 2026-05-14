using System;

namespace HotelBooking.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Number { get; set; } = "";
        public string Type { get; set; } = "";
        public decimal PricePerNight { get; set; }
        public string Description { get; set; } = "";
        public bool IsAvailable { get; set; } = true;
        public int Capacity { get; set; } = 1;
        public string ImagePath { get; set; } = "";

        public override string ToString() => $"Номер {Number} ({Type}) - {PricePerNight:C}/ночь";
    }

    public class Client
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string PassportNumber { get; set; } = "";
    }

    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int ClientId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Status { get; set; } = "Active";
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Room? Room { get; set; }
        public Client? Client { get; set; }

        public int Nights => (CheckOut - CheckIn).Days;
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Role { get; set; } = "Manager";
        public string FullName { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class ChatMessage
    {
        public int Id { get; set; }
        public string Sender { get; set; } = "";
        public string Message { get; set; } = "";
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public bool IsFromClient { get; set; }
    }
}
