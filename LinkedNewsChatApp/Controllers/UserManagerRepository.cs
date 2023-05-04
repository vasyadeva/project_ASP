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

        public async Task<bool> UpdateProfileAsync(int userId,  string email, string region, int avaid, string biography)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (avaid == 0)
            {
                avaid = user.AvatarId;
            }

            if (user == null)
            {
                return false;
            }

            var hubMessages = _context.HubMessages.Where(cm => cm.AvaId == user.AvatarId).ToList();
            if (hubMessages != null)
            {
                foreach (var cm in hubMessages)
                {
                   cm.AvaId = avaid;
                    _context.HubMessages.Update(cm);
                }
            }
            var hubGMessages = _context.hubGroupMessages.Where(cm => cm.AvaId == user.AvatarId).ToList();
            if (hubGMessages != null)
            {
                foreach (var cm in hubGMessages)
                {
                    cm.AvaId = avaid;
                    _context.hubGroupMessages.Update(cm);
                }
            }

            user.Email = email;
            user.Region = region;
            user.AvatarId = avaid;
            user.Biography = biography;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<User> GetUser(string username)
        {
            var user = _context.Users.Where(u => u.Username == username).FirstOrDefault();
            if (user != null)
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);  
            }
            return null; // якщо користувача не знайдено
        }
    }
}
