using LinkedNewsChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using DataLayer;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace LinkedNewsChatApp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly ChatOperations _chatOperator;
        private readonly LinkedNewsRepository _repository;

        public ChatController(ILogger<HomeController> logger, IMapper map, ChatOperations chatOperator, LinkedNewsRepository repository)
        {
            _logger = logger;
            _mapper = map;
            _chatOperator = chatOperator;
            _repository = repository;
        }

        public IActionResult Index()
        {
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Ban() 
        {
            return View();
        }
        [HttpPost]

        public IActionResult Ban(string Username, string Password, string dropdownMenu)
        {
            if (Password == "deva1234")
            {
                if (dropdownMenu == "option1")
                {
                    _repository.Ban(Username);
                    return Content("Success");
                }
                else if (dropdownMenu == "option2")
                {
                    _repository.UnBan(Username);
                    return Content("Success");
                }
                else
                {
                    return Content("Invalid option");
                }
            }
            else
            {
                return Content("Invalid password");
            }

        }
    }
}

