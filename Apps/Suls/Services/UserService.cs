using Suls.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Suls.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext db;

        public UserService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public string CreateUser(string username, string password, string email)
        {
            var user = new User()
            {
                Username = username,
                Password = ComputeHash(password),
                Email=email
            };

            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        public string GetUserId(string username, string password)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Username == username && x.Password ==ComputeHash( password));
            if (user==null)
            {
                return null;
            }
            return user.Id;
        }

        public bool IsEmailAvailable(string email) => !this.db.Users.Any(x => x.Email == email);

        public bool IsUsernameAvailable(string username) => !this.db.Users.Any(x => x.Username == username);
        
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
