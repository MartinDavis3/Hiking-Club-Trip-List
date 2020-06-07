using HikingClubTripList.Data;
using HikingClubTripList.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingClubTripList.Services
{
    public class SignupService : ISignupService
    {
        private readonly ClubContext _context;

        public SignupService(ClubContext context)
        {
            _context = context;
        }

        public async Task<bool> AddSignupAsync(Signup signup)
        {
            _context.Add(signup);
            var addResult = await _context.SaveChangesAsync();
            return addResult == 1;
        }

        public async Task<List<Signup>> GetAllSignupsForTripAsync(int tripID)
        {
            return await _context.Signups
                .Where(s => s.TripID == tripID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Signup> GetSignupAsync(int tripID, int memberID)
        {
            return await _context.Signups
                .FirstOrDefaultAsync(s => s.TripID == tripID && s.MemberID == memberID);

        }

        public async Task<bool> RemoveSignupAsync(Signup signup)
        {
            _context.Signups.Remove(signup);
            var removeResult = await _context.SaveChangesAsync();
            return removeResult == 1;
        }

    }
}
