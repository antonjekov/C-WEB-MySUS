using MySUS.HTTP;
using MySUS.MvcFramework;
using SharedTrip.Services;
using SharedTrip.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace SharedTrip.Controllers
{
    public class TripsController: Controller
    {
        private readonly ITripsService tripsService;
        private readonly IUserTripService userTripService;

        public TripsController(ITripsService tripsService, IUserTripService userTripService)
        {
            this.tripsService = tripsService;
            this.userTripService = userTripService;
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(TripInputModel input)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            if (string.IsNullOrWhiteSpace(input.StartPoint))
            {
                return this.Error("You should choose any start point");
            }
            if (string.IsNullOrWhiteSpace(input.EndPoint))
            {
                return this.Error("You should choose any end point");
            }
            if (input.Seats<2||input.Seats>6)
            {
                return this.Error("Seats must have value between 2 and 6.");
            }
            if (string.IsNullOrWhiteSpace(input.Description)||input.Description.Length>80)
            {
                return this.Error("Description is required and should be less than 80 charackters long");
            }
            if (!Uri.IsWellFormedUriString(input.ImagePath, UriKind.Absolute))
            {
                return this.Error("Please fill valid image url");
            }
            if (!Regex.IsMatch(input.DepartureTime, @"^\d{2}\.\d{2}\.\d{4} \d{2}\:\d{2}$"))
            {
                return this.Error("Departure time format is not correct");
            }

            this.tripsService.CreateTrip(input);

            return this.Redirect("/Trips/All");
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            var trips = this.tripsService.GetAllTrips().Select(x =>
            {
                var specialDate = x.DepartureTime.ToString("dd.MM.yyyyг. HH:mm:ss");
                var freeSeats = x.Seats - this.userTripService.GetCountOfTripOcupiedSeats(x.Id);
                return new TripInputModel()
                {
                    Seats = freeSeats,
                    StartPoint = x.StartPoint,
                    DepartureTime = specialDate,
                    Description = x.Description,
                    EndPoint = x.EndPoint,
                    ImagePath = x.ImagePath,
                    Id = x.Id
                };
            }).ToList(); 

            return this.View(trips);
        }
              

        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            var trip = this.tripsService.GetTripById(tripId);
            var specialDate = trip.DepartureTime.ToString("s");
            var freeSeats = trip.Seats - this.userTripService.GetCountOfTripOcupiedSeats(trip.Id);
            var tripViewModel =  new TripInputModel()
            {
                Seats = freeSeats,
                StartPoint = trip.StartPoint,
                DepartureTime = specialDate,
                Description = trip.Description,
                EndPoint = trip.EndPoint,
                ImagePath = trip.ImagePath,
                Id = trip.Id
            };
           
            return this.View(tripViewModel);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/users/login");
            }
            if (this.userTripService.UserSignedInForTrip(this.GetUserId(), tripId))
            {
                return this.Error("You are already joined to the trip");
            }
            if (!this.tripsService.TripHaveFreeSeats(tripId))
            {
                return this.Error("All the seats for this treap are occupied");
            }
            this.userTripService.Add(this.GetUserId(), tripId);
            return this.Redirect("/Trips/All");
        }

        
    }
}
