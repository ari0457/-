using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using HotelBookingApp.Commands;
using HotelBookingApp.Services;

namespace HotelBookingApp.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        private readonly ChatService _chat;
        private string _inputText;

        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        public string InputText
        {
            get => _inputText;
            set { _inputText = value; OnPropertyChanged(); }
        }

        public ICommand SendCommand { get; }

        public ChatViewModel(ChatService chat)
        {
            _chat = chat;
            _chat.MessageReceived += msg =>
                Application.Current.Dispatcher.Invoke(() => Messages.Add(msg.Display));

            SendCommand = new RelayCommand(async _ =>
            {
                if (string.IsNullOrWhiteSpace(InputText)) return;
                Messages.Add($"[Вы]: {InputText}");
                await _chat.SendAsync(InputText);
                InputText = string.Empty;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}