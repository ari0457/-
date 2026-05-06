using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HotelBookingApp
{
    public class RoomModel : INotifyPropertyChanged
    {
        private bool _isAvailable;

        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal PricePerNight { get; set; }
        public string Description { get; set; }

        public bool IsAvailable
        {
            get => _isAvailable;
            set { _isAvailable = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}