using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HotelBooking.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HotelBooking.Data
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hotel.db");
            options.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany()
                .HasForeignKey(b => b.RoomId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Client)
                .WithMany()
                .HasForeignKey(b => b.ClientId);
        }
    }

    public static class DataSeeder
    {
        public static void Seed(HotelDbContext ctx)
        {
            ctx.Database.EnsureCreated();

            if (!ctx.Rooms.Any())
            {
                var rooms = new List<Room>
                {
                    new() { Number = "101", Type = "Стандарт", PricePerNight = 70, Description = "Уютный стандартный номер с видом на город", IsAvailable = true, Capacity = 1 },
                    new() { Number = "102", Type = "Стандарт", PricePerNight = 70, Description = "Стандартный номер, 2 кровати", IsAvailable = true, Capacity = 2 },
                    new() { Number = "201", Type = "Люкс", PricePerNight = 140, Description = "Просторный люкс с гостиной зоной", IsAvailable = true, Capacity = 2 },
                    new() { Number = "202", Type = "Люкс", PricePerNight = 140, Description = "Люкс с панорамным видом", IsAvailable = true, Capacity = 3 },
                    new() { Number = "301", Type = "Апартаменты", PricePerNight = 240, Description = "Премиум апартаменты с кухней", IsAvailable = true, Capacity = 4 },
                    new() { Number = "302", Type = "Эконом", PricePerNight = 40, Description = "Экономный номер для деловых поездок", IsAvailable = true, Capacity = 1 },
                    new() { Number = "303", Type = "Эконом", PricePerNight = 40, Description = "Компактный эконом номер", IsAvailable = true, Capacity = 1 },
                    new() { Number = "401", Type = "Президентский", PricePerNight = 500, Description = "Президентский люкс высшей категории", IsAvailable = true, Capacity = 2 },
                };
                ctx.Rooms.AddRange(rooms);
                ctx.SaveChanges();
            }

            if (!ctx.Users.Any())
            {
                ctx.Users.Add(new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin",
                    FullName = "Администратор Системы"
                });
                ctx.SaveChanges();
            }
        }
    }
}

namespace HotelBooking.Repositories
{
    using HotelBooking.Data;
    using HotelBooking.Models;

    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }

    public class RoomRepository : IRepository<Room>
    {
        private readonly HotelDbContext _ctx;
        public RoomRepository(HotelDbContext ctx) => _ctx = ctx;

        public async Task<List<Room>> GetAllAsync() =>
            await Task.FromResult(_ctx.Rooms.ToList());

        public async Task<Room?> GetByIdAsync(int id) =>
            await Task.FromResult(_ctx.Rooms.Find(id));

        public async Task AddAsync(Room entity) { _ctx.Rooms.Add(entity); await _ctx.SaveChangesAsync(); }
        public async Task UpdateAsync(Room entity) { _ctx.Rooms.Update(entity); await _ctx.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var r = _ctx.Rooms.Find(id);
            if (r != null) { _ctx.Rooms.Remove(r); await _ctx.SaveChangesAsync(); }
        }
    }

    public class BookingRepository : IRepository<Booking>
    {
        private readonly HotelDbContext _ctx;
        public BookingRepository(HotelDbContext ctx) => _ctx = ctx;

        public async Task<List<Booking>> GetAllAsync() =>
            await Task.FromResult(_ctx.Bookings.Include(b => b.Room).Include(b => b.Client).ToList());

        public async Task<Booking?> GetByIdAsync(int id) =>
            await Task.FromResult(_ctx.Bookings.Include(b => b.Room).Include(b => b.Client).FirstOrDefault(b => b.Id == id));

        public async Task AddAsync(Booking entity) { _ctx.Bookings.Add(entity); await _ctx.SaveChangesAsync(); }
        public async Task UpdateAsync(Booking entity) { _ctx.Bookings.Update(entity); await _ctx.SaveChangesAsync(); }
        public async Task DeleteAsync(int id)
        {
            var b = _ctx.Bookings.Find(id);
            if (b != null) { _ctx.Bookings.Remove(b); await _ctx.SaveChangesAsync(); }
        }
    }
}
