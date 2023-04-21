using LinkedNewsChatApp.Migrations;
using ServiceLayer;
using LinkedNewsChatApp.Data;
using LinkedNewsChatApp.Models;
using DataLayer.Context;
using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace LinkedNewsChatApp.Data
{
    public class UserManagerRepository : IUserManagerRepository
    {
        private readonly LinkedNewsDbContext _context;


        public UserManagerRepository(LinkedNewsDbContext context)
        {
            _context = context;
        }
        public async Task<User> GetCurrentAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<bool> UpdateProfileAsync(int userId, string username, string email, string region, string biography)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return false;
            }

            user.Username = username;
            user.Email = email;
            user.Region = region;
            user.Biography = biography;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
