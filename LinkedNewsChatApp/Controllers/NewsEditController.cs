using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LinkedNewsChatApp.Data;
using LinkedNewsChatApp.Models;
using DataLayer;
using NuGet.Protocol.Core.Types;

namespace LinkedNewsChatApp.Controllers
{
    public class NewsEditController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly NewsRepository _repository;
        public NewsEditController(ApplicationDbContext context, NewsRepository repository)
        {
            _context = context;  
            _repository = repository;
        }
        

        
        // GET: NewsEdit
       
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;
            var news = _context.news.AsQueryable(); ;
         
            int totalNews = await news.CountAsync();
            int totalPages = (int)Math.Ceiling((decimal)totalNews / pageSize);


            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }
            if (totalNews != 0)
            {
                var selectedNews = await news.Skip((page - 1) * pageSize)
                                             .Take(pageSize)
                                             .ToListAsync();
                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentPage = page;
              
                return View(selectedNews);
            }
            else
            {

                return View("Empty");
            }
        }
        // GET: NewsEdit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.news == null)
            {
                return NotFound();
            }

            var news = await _context.news
                .FirstOrDefaultAsync(m => m.id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: NewsEdit/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsEdit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,title,text,Category")] News news, IFormFile photo, string password)
        {
            if (password == "d5n!_4RX")
            {
                if (photo != null)
                {
                    string extension = Path.GetExtension(photo.FileName);
                    if (extension != null &&
                        (extension.ToLower() == ".jpg" ||
                         extension.ToLower() == ".jpeg" ||
                         extension.ToLower() == ".png"))
                    {

                    }
                    else
                    {
                        return BadRequest("Файл повинен бути у форматі jpg, jpeg або png");
                    }
                }


                byte[] photoByte;
                using (var ms = new MemoryStream())
                {
                    photo.CopyTo(ms);
                    byte[] fileBytes = ms.ToArray();
                    photoByte = fileBytes;
                }
                DateTime day = DateTime.Now;
                day = day.AddHours(10);
                var newsobj = new News() { id = news.id, title = news.title, text = news.text, photo = photoByte, Time = day, Category = news.Category };
                // if (ModelState.IsValid)
                // {
                _context.Add(newsobj);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                //}
                //return View(news);
            }
            else
            {
                return View("Incorrect");
            }
        }

        // GET: NewsEdit/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.news == null)
            {
                return NotFound();
            }

            var news = await _context.news.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        // POST: NewsEdit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,title,text,Category")] News news, IFormFile photo, string password)
        {
            if (password == "d5n!_4RX")
            {
                if (id != news.id)
                {
                    return NotFound();
                }



                if (photo != null)
                {
                    string extension = Path.GetExtension(photo.FileName);
                    if (extension != null &&
                        (extension.ToLower() == ".jpg" ||
                         extension.ToLower() == ".jpeg" ||
                         extension.ToLower() == ".png"))
                    {

                    }
                    else
                    {
                        return BadRequest("Файл повинен бути у форматі jpg, jpeg або png");
                    }



                    byte[] photoByte;
                    using (var ms = new MemoryStream())
                    {
                        photo.CopyTo(ms);
                        byte[] fileBytes = ms.ToArray();
                        photoByte = fileBytes;
                    }
                    DateTime day = DateTime.Now;
                    day = day.AddHours(10);
                    var newsobj = new News() { id = news.id, title = news.title, text = news.text, photo = photoByte, Time = day, Category = news.Category };



                    //if (ModelState.IsValid)
                    //{
                    try
                    {
                        _context.Update(newsobj);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!NewsExists(news.id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    var newsoperation = new NewsOperations.NewsOperations(_repository);
                    byte[] phot = _repository.GetNewsPhoto(news.id);
                    DateTime day = DateTime.Now;
                    day = day.AddHours(10);
                    var newsobj = new News() { id = news.id, title = news.title, text = news.text, photo = phot, Time = day, Category = news.Category };



                    //if (ModelState.IsValid)
                    //{
                    try
                    {
                        _context.Update(newsobj);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!NewsExists(news.id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
                // }
                //return View(news);
            }
            else
            {
                return View("Incorrect");
            }
        }

        // GET: NewsEdit/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            
                if (id == null || _context.news == null)
                {
                    return NotFound();
                }

                var news = await _context.news
                    .FirstOrDefaultAsync(m => m.id == id);
                if (news == null)
                {
                    return NotFound();
                }

                return View(news);
            
            
        }

        // POST: NewsEdit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string password)
        {
            if (password == "d5n!_4RX")
            {
                if (_context.news == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.news'  is null.");
                }
                var news = await _context.news.FindAsync(id);
                if (news != null)
                {
                    _context.news.Remove(news);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View("Incorrect");
            }
        }

        public async Task<IActionResult> DataView(int? id)
        {
            if (id == null || _context.news == null)
            {
                return NotFound();
            }

            var news = await _context.news
                .FirstOrDefaultAsync(m => m.id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }
        private bool NewsExists(int id)
        {
          return (_context.news?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
