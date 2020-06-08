using HikingClubTripList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingClubTripList.Services
{
    public interface ITripService
    {
        Task<List<Trip>> GetTripsListAsync();
        Task<Trip> GetTripDetailsAsync(int tripID);
        Task<bool> AddTripAsync(Trip trip);
        Task<Trip> GetTripOnlyAsync(int tripID);
        Task<bool> TripExistsAsync(int tripID);
        Task<bool> UpdateTripAsync(Trip trip);
        Task<Trip> GetTripForDeleteAsync(int tripID);
        Task<bool> RemoveTripAsync(int tripID);
    }
}
