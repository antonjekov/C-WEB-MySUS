using SharedTrip.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedTrip.Services
{
    public class UsersService : IUsersService
    {
        private ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public string CreateUser(string username, string email, string password)
        {
            var user = new User()
            {
                Email = email,
                Password = password,
                Username = username
            };

            this.db.Users.Add(user);
            this.db.SaveChanges();
            return user.Id;
        }

        public string GetUserId(string username, string password) => this.db.Users
            .FirstOrDefault(user => user.Username == username && user.Password == password)?.Id;

        public bool UsernameExist(string username) => this.db.Users.Any(user => user.Username == username);

        public bool EmailExists(string email) => this.db.Users.Any(user => user.Email == email);

    }
}
