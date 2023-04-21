using DataLayer;
using DataLayer.Context;
using LinkedNewsChatApp.Data;
using LinkedNewsChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace LinkedNewsChatApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkedNewsDbContext _context;
        private readonly UserManagerRepository _repository;
        public ProfileController(IHttpContextAccessor httpContextAccessor, LinkedNewsDbContext context, UserManagerRepository repository)

        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _repository = repository;
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _repository.GetCurrentAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var model = new User()
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Region = user.Region,
                Biography = user.Biography
            };

            return View(model);
        } 


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
   
        public async Task<IActionResult> Edit(User model)
        {
            int userId = GetCurrentUserId();

            bool isUpdated = await _repository.UpdateProfileAsync(userId, model.Username, model.Email, model.Region, model.Biography);

            if (isUpdated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Unable to update profile.");
                return View(model);
            }
        }





        public int GetCurrentUserId()
        {
            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (userName == null)
            {
                return -1; // or throw an exception or return null
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == userName);

            if (user == null)
            {
                return -1; // or throw an exception or return null
            }

            return user.UserId;
        }

    }
}
