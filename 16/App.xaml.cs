using System.Windows;
using HotelBookingApp.Services;
using HotelBookingApp.ViewModels;
using HotelBookingApp.Views;

namespace HotelBookingApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var auth = new AuthService();
            var bookingService = new BookingService();
            var notifications = new NotificationService();

            var loginVm = new LoginViewModel(auth);
            var loginWin = new LoginWindow { DataContext = loginVm };

            if (loginWin.ShowDialog() == true && loginVm.LoginSuccessful)
            {
                var mainVm = new HotelViewModel(bookingService, auth, notifications);
                var mainWin = new MainWindow { DataContext = mainVm };
                mainWin.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}