namespace HotelBookingApp.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
    }
}