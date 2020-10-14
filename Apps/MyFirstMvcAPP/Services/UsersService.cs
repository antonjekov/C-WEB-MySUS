using BattleCards.Data;
using Microsoft.EntityFrameworkCore.Internal;
using MySUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BattleCards.Services
{
    public class UsersService : IUsersService
    {
        private ApplicationDbContext db;

        public UsersService()
        {
            this.db = new ApplicationDbContext();
        }

        public string CreateUser(string username, string email, string password)
        {
            var user = new User()
            {
                Username = username,
                Email = email,
                Password = ComputeHash(password),
                Role = IdentityRole.User
            };
            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        public string GetUserId(string username, string password)
        {
            var user =  db.Users.FirstOrDefault(user => user.Username == username && user.Password == ComputeHash(password));
            if (user==null)
            {
                return null;
            }
            return user.Id;
        }

        public bool IsEmailAvailable(string email)
        {
            return !db.Users.Any(u => u.Email == email);
        }

        public bool IsUsernameAvailable(string username)
        {
            return !db.Users.Any(u => u.Username == username);
        }


        private static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var hash = SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }
    }


}
