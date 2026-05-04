namespace HotelBookingApp.Models
{
    public class Room
    {
        public int Number { get; set; }
        public string Type { get; set; }
        public int PricePerNight { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Room(int number, string type, int pricePerNight)
        {
            Number = number;
            Type = type;
            PricePerNight = pricePerNight;
        }

        public override string ToString() =>
            $"№{Number} — {Type} — {PricePerNight} руб./ночь {(IsAvailable ? "[Свободен]" : "[Занят]")}";
    }
}