using System.Windows;
using HotelBooking.Data;

namespace HotelBooking
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            using var ctx = new HotelDbContext();
            DataSeeder.Seed(ctx);
        }
    }
}
