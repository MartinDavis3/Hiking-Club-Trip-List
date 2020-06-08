using HikingClubTripList.Data;
using HikingClubTripList.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingClubTripList.Services
{
    public class TripService : ITripService
    {
        private readonly ClubContext _context;

        public TripService(ClubContext context)
        {
            _context = context;
        }

        public async Task<List<Trip>> GetTripsListAsync()
        {
        return await _context.Trips
                .Include(t => t.Signups)
                    .ThenInclude(s => s.Member)
                .OrderBy(t => t.Date)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Trip> GetTripDetailsAsync(int tripID)
        {
        return await _context.Trips
                .Include(t => t.Signups)
                    .ThenInclude(s => s.Member)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.TripID == tripID);
        }

        public async Task<bool> AddTripAsync(Trip trip)
        {
            _context.Add(trip);
            var addResult = await _context.SaveChangesAsync();
            return addResult == 1;
        }

        public async Task<Trip> GetTripOnlyAsync(int tripID)
        {
            return await _context.Trips.FindAsync(tripID);
        }

        public async Task<bool> TripExistsAsync(int tripID)
        {
            return await _context.Trips.AnyAsync(e => e.TripID == tripID);

        }

        public async Task<bool> UpdateTripAsync(Trip trip)
        {
            _context.Update(trip);
            var updateResult = await _context.SaveChangesAsync();
            return updateResult == 1;
        }

        public async Task<Trip> GetTripForDeleteAsync(int tripID)
        {
            return await _context.Trips
                .FirstOrDefaultAsync(m => m.TripID == tripID);
        }

        public async Task<bool> RemoveTripAsync(int tripID)
        {
            var trip = await _context.Trips.FindAsync(tripID);
            _context.Trips.Remove(trip);
            var deleteResult = await _context.SaveChangesAsync();
            return deleteResult == 1;
        }

    }
}
