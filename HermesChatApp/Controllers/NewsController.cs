using LinkedNewsChatApp.Models;
using Microsoft.AspNetCore.Mvc;
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

        public NewsController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult Index()
        {
            // Get the path of the wwwroot folder
            string wwwrootPath = _hostingEnvironment.WebRootPath;
            // Initialize the dictionary
            IDictionary<int, Tuple<string, string, string>> dictionary = new Dictionary<int, Tuple<string, string, string>>();

            // Read the existing data from the file, if it exists
            string dataFilePath = Path.Combine(wwwrootPath, "data.txt");
            if (System.IO.File.Exists(dataFilePath))
            {
                string[] lines = System.IO.File.ReadAllLines(dataFilePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    int id = int.Parse(parts[0]);
                    dictionary.Add(id, Tuple.Create(parts[1], parts[2], parts[3]));
                }
            }
            return View(dictionary);
        }


        // GET: Home
        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostUsingParameters(PersonData data, IFormFile file, string pass, string FName, string LName)
        {
            if (pass == "deva1234")
            {
                // Get the path of the wwwroot folder
                string wwwrootPath = _hostingEnvironment.WebRootPath;


                // Get the path of the images folder
                string imagesPath = Path.Combine(wwwrootPath, "images");

                // If the images folder doesn't exist, create it
                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }

                // Check if the file was uploaded
                if (file != null && file.Length > 0)
                {
                    // Get the filename of the uploaded file
                    string fileName = Path.GetFileName(file.FileName);

                    // Generate a unique filename
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

                    // Get the full path of the file
                    string filePath = Path.Combine(imagesPath, uniqueFileName);

                    // Save the file to the server
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Set the PhotoPath property of the PersonData object to the filename of the uploaded file
                    data.PhotoPath = "/images/" + uniqueFileName;
                }

                // Initialize the dictionary
                IDictionary<int, Tuple<string, string, string>> dictionary = new Dictionary<int, Tuple<string, string, string>>();

                // Read the existing data from the file, if it exists
                string dataFilePath = Path.Combine(wwwrootPath, "data.txt");
                if (System.IO.File.Exists(dataFilePath))
                {
                    string[] lines = System.IO.File.ReadAllLines(dataFilePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        int id = int.Parse(parts[0]);
                        dictionary.Add(id, Tuple.Create(parts[1], parts[2], parts[3]));
                    }
                }


                // Check if the ID already exists in the dictionary
                int newId = dictionary.Count > 0 ? dictionary.Keys.Max() + 1 : 1;
                while (dictionary.ContainsKey(newId))
                {
                    newId++;
                }

                // Add the new data to the dictionary
                dictionary.Add(newId, Tuple.Create(FName, LName, data.PhotoPath));

                // Write the data to the file
                using (StreamWriter sw = System.IO.File.AppendText(dataFilePath))
                {
                    sw.WriteLine($"{newId},{FName},{LName},{data.PhotoPath}");
                }

                return View(dictionary);
            }
            else
            {
                return Content("Invalid password");
            }

        }
    }
}
