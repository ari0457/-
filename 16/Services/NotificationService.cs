using System;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class NotificationService : IDisposable
    {
        private const string MapName = "HotelNotifications";
        private const int MapSize = 1024;
        private MemoryMappedFile _mmf;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private string _lastMessage = "";

        public event Action<string> NotificationReceived;

        public NotificationService()
        {
            try { _mmf = MemoryMappedFile.CreateOrOpen(MapName, MapSize); }
            catch { _mmf = MemoryMappedFile.OpenExisting(MapName); }
        }

        public void Publish(string message)
        {
            // using без скобок заменён на using со скобками (C# 7 совместимо)
            using (var accessor = _mmf.CreateViewAccessor(0, MapSize))
            {
                var bytes = Encoding.UTF8.GetBytes(message.PadRight(512, '\0'));
                accessor.WriteArray(0, bytes, 0, Math.Min(bytes.Length, MapSize));
            }
        }

        public void StartListening()
        {
            Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    using (var accessor = _mmf.CreateViewAccessor(0, MapSize))
                    {
                        var bytes = new byte[MapSize];
                        accessor.ReadArray(0, bytes, 0, MapSize);
                        var msg = Encoding.UTF8.GetString(bytes).TrimEnd('\0').Trim();

                        if (!string.IsNullOrEmpty(msg) && msg != _lastMessage)
                        {
                            _lastMessage = msg;
                            NotificationReceived?.Invoke(msg);
                        }
                    }
                    await Task.Delay(1000, _cts.Token);
                }
            }, _cts.Token);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _mmf?.Dispose();
        }
    }
}