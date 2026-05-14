using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HotelBooking.Data;
using HotelBooking.Services;
using HotelBooking.ViewModels;

namespace HotelBooking.Views
{
    public partial class MainWindow : Window
    {
        private readonly HotelViewModel _vm;
        private readonly AuthService _authService;
        private readonly ChatService _chatService;
        private readonly HotelDbContext _ctx;

        public MainWindow(AuthService authService)
        {
            InitializeComponent();
            _authService = authService;
            _ctx = new HotelDbContext();
            var bookingService = new BookingService(_ctx);
            _vm = new HotelViewModel(_ctx, bookingService);
            DataContext = _vm;

            UserLabel.Text = $"👤 {authService.CurrentUser?.FullName} ({authService.CurrentUser?.Role})";

            _chatService = new ChatService();
            _chatService.MessageReceived += OnChatMessageReceived;
            _chatService.StartServer();

            _vm.PropertyChanged += OnVmPropertyChanged;

            AddChatMessage("Система", "Чат готов. Ожидание сообщений от клиентов...", false);
        }

        private void OnVmPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HotelViewModel.IsBusy))
            {
                LoadingOverlay.Visibility = _vm.IsBusy ? Visibility.Visible : Visibility.Collapsed;
                if (_vm.IsBusy) LoadingText.Text = _vm.BusyMessage;
            }
        }

        private void OnChatMessageReceived(Models.ChatMessage msg)
        {
            Dispatcher.Invoke(() => AddChatMessage(msg.Sender, msg.Message, msg.IsFromClient));
        }

        private void AddChatMessage(string sender, string message, bool isFromClient)
        {
            var container = new Border
            {
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(10, 6, 10, 6),
                Margin = new Thickness(0, 3, 0, 3),
                Background = isFromClient
                    ? new SolidColorBrush(Color.FromRgb(219, 234, 254))
                    : new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                HorizontalAlignment = isFromClient ? HorizontalAlignment.Left : HorizontalAlignment.Right,
                MaxWidth = 260
            };
            var sp = new StackPanel();
            sp.Children.Add(new TextBlock
            {
                Text = sender,
                FontSize = 10,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(100, 116, 139))
            });
            sp.Children.Add(new TextBlock { Text = message, FontSize = 12, TextWrapping = TextWrapping.Wrap });
            sp.Children.Add(new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm"),
                FontSize = 10,
                Foreground = new SolidColorBrush(Color.FromRgb(148, 163, 184)),
                HorizontalAlignment = HorizontalAlignment.Right
            });
            container.Child = sp;

            // Animate entrance
            container.Opacity = 0;
            ChatPanel.Children.Add(container);
            ChatScroll.ScrollToBottom();

            var anim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            container.BeginAnimation(OpacityProperty, anim);
        }

        private void ChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SendChat_Click(sender, new RoutedEventArgs());
        }

        private async void SendChat_Click(object sender, RoutedEventArgs e)
        {
            var msg = ChatInput.Text.Trim();
            if (string.IsNullOrEmpty(msg)) return;
            ChatInput.Clear();
            AddChatMessage(_authService.CurrentUser?.FullName ?? "Менеджер", msg, false);
            await _chatService.SendMessageAsync(msg, _authService.CurrentUser?.FullName ?? "Менеджер");
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _authService.Logout();
            new LoginWindow().Show();
            Close();
        }

        private void ShowRooms_Click(object sender, RoutedEventArgs e)
        {
            // Already shown in main view; could switch tab
        }

        private void ShowAvailableRooms_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Доступных номеров: {_vm.AvailableRooms.Count}", "Свободные номера");
        }

        private void ShowSettings_Click(object sender, RoutedEventArgs e)
        {
            var win = new SettingsWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        protected override void OnClosed(EventArgs e)
        {
            _chatService?.Dispose();
            _ctx?.Dispose();
            base.OnClosed(e);
        }
    }
}
