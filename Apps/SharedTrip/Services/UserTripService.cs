using Microsoft.EntityFrameworkCore.Internal;
using SharedTrip.Data;
using System.Linq;

namespace SharedTrip.Services
{
    public class UserTripService : IUserTripService
    {
        private readonly ApplicationDbContext db;

        public UserTripService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Add(string userId, string tripId)
        {
            var userTrip = new UserTrip()
            {
                TripId = tripId,
                UserId = userId
            };
            this.db.UserTrips.Add(userTrip);
            this.db.SaveChanges();
            
        }

        public bool UserSignedInForTrip(string userId, string tripId) => this.db.UserTrips.Any(userTrip => userTrip.UserId == userId && userTrip.TripId == tripId);

        public int GetCountOfTripOcupiedSeats(string tripId) => this.db.UserTrips.Where(x => x.TripId == tripId).Count();
    }
}
