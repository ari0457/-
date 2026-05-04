using HotelBookingApp.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HotelBookingApp
{
    public partial class BookingWindow : Window
    {
        public Room SelectedRoom { get; private set; }
        public string GuestName { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public int TotalPrice { get; private set; }

        private readonly List<Room> availableRooms;

        public BookingWindow(List<Room> rooms, Booking booking = null)
        {
            InitializeComponent();
            availableRooms = rooms;
            RoomCombo.ItemsSource = availableRooms;

            if (booking != null)
            {
                GuestNameBox.Text = booking.GuestName;
                StartDatePicker.SelectedDate = booking.StartDate;
                EndDatePicker.SelectedDate = booking.EndDate;
                RoomCombo.SelectedIndex = availableRooms
                    .FindIndex(r => r.Number == booking.RoomNumber);
            }
            else
            {
                RoomCombo.SelectedIndex = 0;
            }

            RoomCombo.SelectionChanged += (s, e) => RecalcPrice();
            StartDatePicker.SelectedDateChanged += (s, e) => RecalcPrice();
            EndDatePicker.SelectedDateChanged += (s, e) => RecalcPrice();
            SaveButton.Click += SaveButton_Click;
            CancelButton.Click += (s, e) => { DialogResult = false; };
        }

        private void RecalcPrice()
        {
            if (RoomCombo.SelectedItem is Room room &&
                StartDatePicker.SelectedDate.HasValue &&
                EndDatePicker.SelectedDate.HasValue)
            {
                int nights = (EndDatePicker.SelectedDate.Value
                            - StartDatePicker.SelectedDate.Value).Days;
                TotalPrice = nights > 0 ? nights * room.PricePerNight : 0;
                TotalPriceText.Text = $"Стоимость: {TotalPrice} руб.";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorText.Text = string.Empty;

            if (RoomCombo.SelectedItem == null)
            { ErrorText.Text = "Выберите номер!"; return; }

            if (string.IsNullOrWhiteSpace(GuestNameBox.Text))
            { ErrorText.Text = "Введите ФИО гостя!"; return; }

            if (!StartDatePicker.SelectedDate.HasValue)
            { ErrorText.Text = "Выберите дату заезда!"; return; }

            if (!EndDatePicker.SelectedDate.HasValue)
            { ErrorText.Text = "Выберите дату выезда!"; return; }

            var start = StartDatePicker.SelectedDate.Value;
            var end = EndDatePicker.SelectedDate.Value;

            if (start < DateTime.Today)
            { ErrorText.Text = "Дата заезда не может быть в прошлом!"; return; }

            if (end <= start)
            { ErrorText.Text = "Дата выезда должна быть позже заезда!"; return; }

            SelectedRoom = (Room)RoomCombo.SelectedItem;
            GuestName = GuestNameBox.Text.Trim();
            StartDate = start;
            EndDate = end;
            RecalcPrice();

            DialogResult = true;
        }
    }
}