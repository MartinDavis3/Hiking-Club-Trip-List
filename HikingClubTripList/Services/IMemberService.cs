using HikingClubTripList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingClubTripList.Services
{
    public interface IMemberService
    {
        Task<Member> GetLoggedInMemberAsync();
        Task<Member> GetValidMemberAsync(Login login);
        Task<bool> ChangeMemberLoginStateAsync(Member member);
        Member GetLoggedInMember();
    }
}
