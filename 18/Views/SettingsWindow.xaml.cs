using System.Windows;

namespace HotelBooking.Views
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            HotelNameBox.Text = "Grand Hotel";
            AddressBox.Text = "ул. Советская, 1, Гродно";

            for (int h = 0; h <= 23; h++)
            {
                CheckInCombo.Items.Add($"{h:00}:00");
                CheckOutCombo.Items.Add($"{h:00}:00");
            }
            CheckInCombo.SelectedIndex = 14;
            CheckOutCombo.SelectedIndex = 12;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Настройки сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
        }
    }
}
