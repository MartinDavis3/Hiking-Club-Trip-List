using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HikingClubTripList.Data;
using HikingClubTripList.Models;
using System.Diagnostics;

namespace HikingClubTripList.Services
{
    public class MemberService : IMemberService
    {
        private readonly ClubContext _context;
        public MemberService(ClubContext context)
        {
            _context = context;
        }
        public async Task<Member> GetLoggedInMemberAsync()
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.IsLoggedIn);
        }
        public async Task<Member> GetValidMemberAsync(Login login)
        {
            return await _context.Members
                .FirstOrDefaultAsync(m => m.Username == login.Username && m.Password == login.Password);

        }
        public async Task<bool> ChangeMemberLoginStateAsync(Member member)
        {
            _context.Update(member);
            var updateResult = await _context.SaveChangesAsync();
            return updateResult == 1;
        }
    }
}
