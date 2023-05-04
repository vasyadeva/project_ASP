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

        public IActionResult Ban(string Username, string Password, string dropdownMenu, string numericField)
        {
            if (Password == "d5n!_4RX")
            {
                if (dropdownMenu == "option1")
                {
                    int term = Convert.ToInt32(numericField);

                    _repository.Ban(Username, term); return View("Success");

                }
                else if (dropdownMenu == "option2")
                {
                    _repository.UnBan(Username);
                    return View("Success");
                }
                else
                {
                    return Content("Invalid option");
                }
            }
            else
            {
                return View("Incorrect");
            }

        }

        [HttpGet]
        public IActionResult DeleteMessage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DeleteMessage(string Username, string Password, string Message, string dropdownMenu)
        {
            if (Password == "d5n!_4RX")
            {
                if (dropdownMenu == "Одне повідомлення")
                {
                    
                    _repository.DeleteMessage(Username, Message);
                    return View("Success");

                }
                else if (dropdownMenu == "Усі повідомлення")
                {
                    _repository.DeleteMessages(Username, Message);
                    return View("Success");
                }
                else
                {
                    return Content("Invalid option");
                }
            }
            else
            {
                return View("Incorrect");
            }

        }
    }
}

