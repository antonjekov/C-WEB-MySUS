using SharedTrip.Data;
using SharedTrip.ViewModels;
using System.Collections.Generic;

namespace SharedTrip.Services
{
    public interface ITripsService
    {
        string CreateTrip(TripInputModel input);
        IEnumerable<Trip> GetAllTrips();
        Trip GetTripById(string tripId);
        bool TripHaveFreeSeats(string tripId);
    }
}