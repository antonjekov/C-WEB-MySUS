namespace SharedTrip.Services
{
    public interface IUsersService
    {
        string CreateUser(string username, string email, string password);
        bool EmailExists(string email);
        string GetUserId(string username, string password);
        bool UsernameExist(string username);
    }
}