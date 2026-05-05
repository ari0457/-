using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HotelBookingApp.Models
{
    public class Room : INotifyPropertyChanged
    {
        private bool isAvailable = true;

        public int Number { get; set; }
        public string Type { get; set; }
        public int PricePerNight { get; set; }

        public bool IsAvailable
        {
            get => isAvailable;
            set { isAvailable = value; OnPropertyChanged(); OnPropertyChanged(nameof(StatusText)); OnPropertyChanged(nameof(StatusColor)); }
        }

        public string StatusText => IsAvailable ? "Свободен" : "Занят";
        public string StatusColor => IsAvailable ? "#27AE60" : "#E74C3C";

        public Room(int number, string type, int pricePerNight)
        {
            Number = number;
            Type = type;
            PricePerNight = pricePerNight;
        }

        public override string ToString() => $"№{Number} — {Type} — {PricePerNight} руб./ночь";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}