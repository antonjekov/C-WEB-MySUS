using MySUS.HTTP;
using MySUS.MvcFramework;
using SharedTrip.Services;
using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
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
            var userId = usersService.GetUserId(username, password);
            if (userId==null)
            {
                return this.Error("Invalid username or password");                
            }
            this.SignIn(userId);
            return this.Redirect("/");
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
        public HttpResponse Register(string username,string email, string password, string confirmPassword)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            if (password!=confirmPassword)
            {
                return this.Error("Password and confirm password dosn't match");
            }
            if (string.IsNullOrWhiteSpace(username)||username.Length<5|| username.Length>20)
            {
                return this.Error("Username should be between 5 and 20 characters long");
            }
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6 || password.Length > 20)
            {
                return this.Error("Password should be between 6 and 20 characters long");
            }
            if (string.IsNullOrWhiteSpace(email)|| !new EmailAddressAttribute().IsValid(email))
            {
                return this.Error("Email is not valid");
            }
            if (usersService.EmailExists(email))
            {
                return this.Error("Email exists");
            }
            if (usersService.UsernameExist(username))
            {
                return this.Error("Username exists");
            }

            usersService.CreateUser(username, email, password);

            return this.Redirect("/users/login");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Error("Only signed in users can signout");
            }
            this.SignOut();
            return this.Redirect("/");
        }
    }
}
