using System;
using System.Collections.Generic;
using System.Windows;

namespace HotelBookingApp
{
    public partial class MainWindow : Window
    {
        private List<Room> rooms;

        public MainWindow()
        {
            InitializeComponent();
            LoadRooms();
            BookButton.Click += BookButton_Click;
        }

        private void LoadRooms()
        {
            rooms = new List<Room>
            {
                new Room(101, "Стандарт", 2500),
                new Room(102, "Стандарт", 2500),
                new Room(201, "Люкс", 4500),
                new Room(202, "Люкс", 4500),
                new Room(301, "Семейный", 3500),
                new Room(302, "Семейный", 3500)
            };
            RoomsGrid.ItemsSource = rooms;
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            MessageText.Text = "";

            if (string.IsNullOrWhiteSpace(GuestNameBox.Text))
            {
                MessageText.Text = "Ошибка: Введите ФИО гостя!";
                GuestNameBox.Focus();
                return;
            }

            if (StartDatePicker.SelectedDate == null)
            {
                MessageText.Text = "Ошибка: Выберите дату заезда!";
                StartDatePicker.Focus();
                return;
            }

            if (EndDatePicker.SelectedDate == null)
            {
                MessageText.Text = "Ошибка: Выберите дату выезда!";
                EndDatePicker.Focus();
                return;
            }

            DateTime start = StartDatePicker.SelectedDate.Value;
            DateTime end = EndDatePicker.SelectedDate.Value;

            if (end <= start)
            {
                MessageText.Text = "Ошибка: Дата выезда должна быть позже даты заезда!";
                EndDatePicker.Focus();
                return;
            }

            if (start < DateTime.Today)
            {
                MessageText.Text = "Ошибка: Дата заезда не может быть в прошлом!";
                StartDatePicker.Focus();
                return;
            }

            if (RoomsGrid.SelectedItem == null)
            {
                MessageText.Text = "Ошибка: Выберите номер из списка!";
                return;
            }

            Room selectedRoom = (Room)RoomsGrid.SelectedItem;
            int nights = (end - start).Days;
            int totalPrice = nights * selectedRoom.PricePerNight;

            MessageText.Foreground = System.Windows.Media.Brushes.Green;
            MessageText.Text = $"Успешно! {GuestNameBox.Text}, вы забронировали номер {selectedRoom.Number} " +
                               $"на {nights} ночей.\nОбщая стоимость: {totalPrice} руб.";
        }
    }
}