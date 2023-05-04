using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkedNewsChatApp.Data
{
    public interface IUserManagerRepository
    {
        public Task<User> GetCurrentAsync(int userId);
        public Task<bool> UpdateProfileAsync(int userId, string email, string region, int avaid, string biography);

        public Task<User> GetUser(string username);


    }

}