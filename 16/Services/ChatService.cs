using System;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public string Display => $"[{Time:HH:mm}] {Sender}: {Text}";
    }

    public class ChatService : IDisposable
    {
        private const string PipeName = "HotelChatPipe";
        private NamedPipeServerStream _server;
        private NamedPipeClientStream _client;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public event Action<ChatMessage> MessageReceived;
        public string Username { get; }
        public bool IsServer { get; }

        public ChatService(string username, bool isServer)
        {
            Username = username;
            IsServer = isServer;
        }

        public async Task StartServerAsync()
        {
            _server = new NamedPipeServerStream(PipeName, PipeDirection.InOut,
                1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            await _server.WaitForConnectionAsync(_cts.Token);
            _ = ListenAsync(_server);
        }

        public async Task ConnectAsync()
        {
            _client = new NamedPipeClientStream(".", PipeName,
                PipeDirection.InOut, PipeOptions.Asynchronous);
            await _client.ConnectAsync(5000, _cts.Token);
            _ = ListenAsync(_client);
        }

        public async Task SendAsync(string text)
        {
            var msg = $"{Username}|{text}|{DateTime.Now:O}";
            var bytes = Encoding.UTF8.GetBytes(msg);
            PipeStream pipe = IsServer ? (PipeStream)_server : _client;
            if (pipe?.IsConnected == true)
                await pipe.WriteAsync(bytes, 0, bytes.Length);
        }

        private async Task ListenAsync(PipeStream pipe)
        {
            var buf = new byte[4096];
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    int read = await pipe.ReadAsync(buf, 0, buf.Length, _cts.Token);
                    if (read == 0) break;
                    var raw = Encoding.UTF8.GetString(buf, 0, read);
                    var parts = raw.Split('|');
                    if (parts.Length >= 3)
                    {
                        MessageReceived?.Invoke(new ChatMessage
                        {
                            Sender = parts[0],
                            Text = parts[1],
                            Time = DateTime.Parse(parts[2])
                        });
                    }
                }
                catch { break; }
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _server?.Dispose();
            _client?.Dispose();
        }
    }
}