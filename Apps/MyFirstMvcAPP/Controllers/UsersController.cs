using BattleCards.Data;
using BattleCards.Services;
using MySUS.HTTP;
using MySUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BattleCards.Controllers
{
    public class UsersController: Controller
    {
        private UsersService usersService;

        public UsersController()
        {
            this.usersService = new UsersService();
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost("/users/login")]
        public HttpResponse DoLogin()
        {
            //read data
            var username = this.Request.FormData["username"];
            var password = this.Request.FormData["password"];
            //check user
            var user = this.usersService.GetUserId(username, password);
            if (user == null)
            {
                return this.Error("Invalid username or password");
            }
            //log user
            this.SignIn(user);            
            //redirect
            return this.Redirect("/cards/all");
        }

        public HttpResponse Register()
        {
            return this.View();            
        }

        [HttpPost("/users/register")]
        public HttpResponse DoRegister()
        {
            var username = this.Request.FormData["username"];
            var password = this.Request.FormData["password"];
            var email = this.Request.FormData["email"];
            var confirmPassword = this.Request.FormData["confirmPassword"];

            if (username==null||username.Length<5||username.Length>20)
            {
                return this.Error("Username should be between 5 and 20 symbols.");
            }

            if (password == null || password.Length < 6 || password.Length > 20)
            {
                return this.Error("Password should be between 6 and 20 symbols.");
            }

            if (!Regex.IsMatch(username, @"^[a-zA-Z0-9\.]+$"))
            {
                return this.Error("Invalid username. Only alphanumeric characters are allowed.");
            }

            if (string.IsNullOrWhiteSpace(email)|| !new EmailAddressAttribute().IsValid(email))
            {
                return this.Error("Email is not valid.");
            }

            if (password!=confirmPassword)
            {
                return this.Error("Passwords should be the same.");
            }

            if (!usersService.IsUsernameAvailable(username))
            {
                return this.Error("Username already exists. Choose other username.");
            }

            if (!usersService.IsEmailAvailable(email))
            {
                return this.Error("Email already exists. Choose other email.");
            }

            usersService.CreateUser(username, email, password);
            return this.Redirect("/users/login");
        }

        // /users/logout
        public HttpResponse Logout()
        {
            if (this.IsUserSignedIn())
            {
            this.SignOut();
            return this.Redirect("/");
            }
            else
            {
                return this.Error("Only signed in users can logout.");
            }
        }
    }
}
