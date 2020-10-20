namespace SharedTrip.Services
{
    public interface IUserTripService
    {
        void Add(string userId, string tripId);

        bool UserSignedInForTrip(string userId, string tripId);

        int GetCountOfTripOcupiedSeats(string tripId);
    }
}