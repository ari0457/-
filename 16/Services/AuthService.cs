using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using HotelBookingApp.Models;

namespace HotelBookingApp.Services
{
    public class AuthService
    {
        private readonly string _usersFile = "users.json";
        private List<UserModel> _users = new List<UserModel>();

        public UserModel CurrentUser { get; private set; }

        public AuthService()
        {
            Load();
            if (!_users.Any(u => u.Role == "Manager"))
            {
                _users.Add(new UserModel
                {
                    Username = "admin",
                    PasswordHash = Hash("admin123"),
                    Role = "Manager",
                    FullName = "Администратор"
                });
                Save();
            }
        }

        public bool Login(string username, string password)
        {
            var user = _users.FirstOrDefault(u =>
                u.Username == username && u.PasswordHash == Hash(password));
            if (user == null) return false;
            CurrentUser = user;
            return true;
        }

        public bool Register(string username, string password, string fullName, string role = "Guest")
        {
            if (_users.Any(u => u.Username == username)) return false;
            _users.Add(new UserModel
            {
                Username = username,
                PasswordHash = Hash(password),
                Role = role,
                FullName = fullName
            });
            Save();
            return true;
        }

        public void Logout() => CurrentUser = null;

        private string Hash(string input)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }

        private void Load()
        {
            if (!File.Exists(_usersFile)) return;
            var json = File.ReadAllText(_usersFile);
            _users = JsonConvert.DeserializeObject<List<UserModel>>(json)
                     ?? new List<UserModel>();
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(_users, Formatting.Indented);
            File.WriteAllText(_usersFile, json);
        }
    }
}