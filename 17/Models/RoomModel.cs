using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HotelAnimatedApp.Models
{
    public class RoomModel : INotifyPropertyChanged
    {
        private bool _isAvailable;
        private bool _isSelected;

        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public decimal PricePerNight { get; set; }
        public string Description { get; set; }
        public string ImageIcon { get; set; }

        public bool IsAvailable
        {
            get => _isAvailable;
            set { _isAvailable = value; OnPropertyChanged(); }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}