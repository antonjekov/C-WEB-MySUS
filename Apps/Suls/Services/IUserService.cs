using Suls.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suls.Services
{
    public interface IUserService
    {
        string CreateUser(string username, string password, string email);

        string GetUserId(string username, string password);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}
