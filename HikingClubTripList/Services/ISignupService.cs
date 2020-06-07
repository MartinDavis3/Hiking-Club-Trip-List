using HikingClubTripList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HikingClubTripList.Services
{
    public interface ISignupService
    {
        Task<bool> AddSignupAsync(Signup signup);
        Task<List<Signup>> GetAllSignupsForTripAsync(int tripID);
        Task<Signup> GetSignupAsync(int tripID, int memberID);
        Task<bool> RemoveSignupAsync(Signup signup);
    }
}
