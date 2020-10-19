using Microsoft.CodeAnalysis.CSharp.Syntax;
using MySUS.HTTP;
using MySUS.MvcFramework;
using Suls.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Suls.Controllers
{
    public class UsersController : Controller
    {
        private IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public HttpResponse Login()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            if (string.IsNullOrWhiteSpace(username) || username.Length < 5 || username.Length > 20)
            {
                return this.Error("Username should be between 5 and 20 characters long.");
            }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6 || password.Length > 20)
            {
                return this.Error("Password should be between 6 and 20 characters long.");
            }
            var userId = this.userService.GetUserId(username, password);
            if (userId==null)
            {
                return this.Error("Invalid username or password");
            }
            this.SignIn(userId);
            return Redirect("/");
        }

        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(string username, string password, string confirmPassword, string email)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            if (string.IsNullOrWhiteSpace(username)||username.Length<5||username.Length>20)
            {
                return this.Error("Username should be between 5 and 20 characters long.");
            }
            if (string.IsNullOrWhiteSpace(password)||password.Length<6||password.Length>20)
            {
                return this.Error("Password should be between 6 and 20 characters long.");
            }
            if (string.IsNullOrWhiteSpace(email)||!new EmailAddressAttribute().IsValid(email))
            {
                return this.Error("Email is not valid.");
            }
            if (!this.userService.IsUsernameAvailable(username))
            {
                return this.Error("This username is already registered.");
            }
            if (!this.userService.IsEmailAvailable(email))
            {
                return this.Error("This email is already registered.");
            }
            if (password!=confirmPassword)
            {
                return this.Error("Password and ConfirmPassword dosn't match.");
            }
           
            this.userService.CreateUser(username, password, email);
            return this.Redirect("/users/login");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            this.SignOut();
            return Redirect("/");
        }
    }
}
