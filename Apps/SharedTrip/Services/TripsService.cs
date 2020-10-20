using SharedTrip.Data;
using SharedTrip.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SharedTrip.Services
{
    public class TripsService : ITripsService
    {
        private readonly ApplicationDbContext db;
        private readonly IUserTripService userTripService;

        public TripsService(ApplicationDbContext db, IUserTripService userTripService)
        {
            this.db = db;
            this.userTripService = userTripService;
        }

        public string CreateTrip(TripInputModel input)
        {
            var trip = new Trip
            {
                Seats = input.Seats,
                StartPoint = input.StartPoint,
                DepartureTime = DateTime.ParseExact(input.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                Description = input.Description,
                EndPoint = input.EndPoint,
                ImagePath = input.ImagePath
            };
            this.db.Trips.Add(trip);
            db.SaveChanges();
            return trip.Id;
        }

        public IEnumerable<Trip> GetAllTrips() => this.db.Trips.ToList();
        
        public Trip GetTripById(string tripId) => this.db.Trips.FirstOrDefault(x => x.Id == tripId);
        
        public bool TripHaveFreeSeats(string tripId)
        {
            var trip = this.db.Trips.FirstOrDefault(trip => trip.Id == tripId);
            if (trip != null && trip.Seats-this.userTripService.GetCountOfTripOcupiedSeats(tripId) > 0)
            {
                return true;
            }
            return false;
        }

    }
}
