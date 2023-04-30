﻿using DataLayer.Entities;
using LinkedNewsChatApp.Data;
using LinkedNewsChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace LinkedNewsChatApp.Controllers
{
    public class NewsController : Controller
    {

       

        List<string> data = new List<string>();
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;

        public NewsController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;
            var news = _context.news.AsQueryable();
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

            var selectedNews = await news.Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(selectedNews);

        }
        public async Task<IActionResult> Data(int? id)
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

    }
    
}
