using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkedNewsChatApp.Data
{
    public interface IUserManagerRepository
    {
        public Task<User> GetCurrentAsync(int userId);
        public Task<bool> UpdateProfileAsync(int userId, string username, string email, string region, string biography);
       
    }

}