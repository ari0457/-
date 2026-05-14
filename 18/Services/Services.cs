using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HotelBooking.Data;
using HotelBooking.Models;
using HotelBooking.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Services
{
    public class BookingService
    {
        private readonly HotelDbContext _ctx;
        private readonly BookingRepository _bookingRepo;
        private readonly RoomRepository _roomRepo;

        public BookingService(HotelDbContext ctx)
        {
            _ctx = ctx;
            _bookingRepo = new BookingRepository(ctx);
            _roomRepo = new RoomRepository(ctx);
        }

        public async Task<Booking> BookRoomAsync(int roomId, Client client, DateTime checkIn, DateTime checkOut)
        {
            await Task.Delay(2000);

            var room = await _roomRepo.GetByIdAsync(roomId)
                ?? throw new Exception("Номер не найден");

            if (!room.IsAvailable)
                throw new Exception("Номер уже забронирован");

            var existingClient = _ctx.Clients.FirstOrDefault(c => c.Phone == client.Phone);
            if (existingClient == null)
            {
                _ctx.Clients.Add(client);
                await _ctx.SaveChangesAsync();
                existingClient = client;
            }

            var nights = (checkOut - checkIn).Days;
            if (nights <= 0) throw new Exception("Дата выезда должна быть позже даты заезда");

            var booking = new Booking
            {
                RoomId = room.Id,
                ClientId = existingClient.Id,
                CheckIn = checkIn,
                CheckOut = checkOut,
                TotalPrice = room.PricePerNight * nights,
                Status = "Active",
                CreatedAt = DateTime.Now,
                Room = room,
                Client = existingClient
            };

            room.IsAvailable = false;
            _ctx.Rooms.Update(room);
            await _bookingRepo.AddAsync(booking);

            NotifyBookingChange($"Бронирование создано: Номер {room.Number}, Клиент {existingClient.FullName}");

            return booking;
        }

        public async Task CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId)
                ?? throw new Exception("Бронирование не найдено");

            booking.Status = "Cancelled";
            _ctx.Bookings.Update(booking);

            var room = await _roomRepo.GetByIdAsync(booking.RoomId);
            if (room != null)
            {
                room.IsAvailable = true;
                _ctx.Rooms.Update(room);
            }

            await _ctx.SaveChangesAsync();
            NotifyBookingChange($"Бронирование отменено: ID {bookingId}");
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            var nights = (booking.CheckOut - booking.CheckIn).Days;
            if (nights <= 0) throw new Exception("Дата выезда должна быть позже даты заезда");

            if (booking.Room != null)
                booking.TotalPrice = booking.Room.PricePerNight * nights;

            _ctx.Bookings.Update(booking);
            await _ctx.SaveChangesAsync();
            NotifyBookingChange($"Бронирование обновлено: ID {booking.Id}");
        }

        private void NotifyBookingChange(string message)
        {
            try
            {
                var mapName = "HotelBookingNotifications";
                using var mmf = MemoryMappedFile.CreateOrOpen(mapName, 1024);
                using var accessor = mmf.CreateViewAccessor();
                var bytes = Encoding.UTF8.GetBytes(message.PadRight(256).Substring(0, 256));
                accessor.WriteArray(0, bytes, 0, bytes.Length);
            }
            catch { /* Non-critical */ }
        }
    }

    public class AuthService
    {
        private readonly HotelDbContext _ctx;
        public User? CurrentUser { get; private set; }

        public AuthService(HotelDbContext ctx) => _ctx = ctx;

        public bool Login(string username, string password)
        {
            var user = _ctx.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return false;
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return false;
            CurrentUser = user;
            return true;
        }

        public bool Register(string username, string password, string fullName, string role = "Manager")
        {
            if (_ctx.Users.Any(u => u.Username == username)) return false;
            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                FullName = fullName,
                Role = role
            };
            _ctx.Users.Add(user);
            _ctx.SaveChanges();
            return true;
        }

        public void Logout() => CurrentUser = null;
        public bool IsLoggedIn => CurrentUser != null;
    }

    public class ChatService : IDisposable
    {
        private NamedPipeServerStream? _pipeServer;
        private CancellationTokenSource? _cts;
        public event Action<ChatMessage>? MessageReceived;

        public void StartServer()
        {
            _cts = new CancellationTokenSource();
            _ = ListenAsync(_cts.Token);
        }

        private async Task ListenAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    _pipeServer = new NamedPipeServerStream("HotelChat", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                    await _pipeServer.WaitForConnectionAsync(token);

                    var buffer = new byte[1024];
                    var bytesRead = await _pipeServer.ReadAsync(buffer, 0, buffer.Length, token);
                    var msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    MessageReceived?.Invoke(new ChatMessage
                    {
                        Sender = "Клиент",
                        Message = msg,
                        IsFromClient = true,
                        Timestamp = DateTime.Now
                    });

                    _pipeServer.Disconnect();
                }
                catch (OperationCanceledException) { break; }
                catch { await Task.Delay(1000); }
            }
        }

        public async Task SendMessageAsync(string message, string sender)
        {
            try
            {
                using var client = new NamedPipeClientStream(".", "HotelChatReply", PipeDirection.Out);
                await client.ConnectAsync(500);
                var bytes = Encoding.UTF8.GetBytes(message);
                await client.WriteAsync(bytes, 0, bytes.Length);
            }
            catch { /* Client not listening */ }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _pipeServer?.Dispose();
        }
    }
}
