using HermesChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace HermesChatApp.Controllers
{
    public class NewsController : Controller
    {

        private readonly TelegramBotClient _botClient;
        private readonly IWebHostEnvironment _env;

        public NewsController(IWebHostEnvironment env)
        {
            // Ініціалізуємо клієнт TelegramBotClient з використанням API ключа
            _botClient = new TelegramBotClient("6186923370:AAHipk9pGebrcpbBKFuCLKvBGfB6-c3dG8o");
            _env = env;
        }


        public async Task<IActionResult> Index()
        {
            var webRoot = _env.WebRootPath;
            var photosPath = Path.Combine(webRoot, "photos");
            Directory.CreateDirectory(photosPath);

            // Отримуємо список повідомлень з каналу
            var updates = await _botClient.GetUpdatesAsync();

            // Формуємо список текстів повідомлень та фото
            var messages = updates.Select(u => u.Message?.Text).Where(t => t != null).ToList();

            var photos = new List<string>();

            foreach (var update in updates)
            {
                var message = update.Message;

                if (message != null)
                {
                    var text = message.Text;
                    var photo = message.Photo?.LastOrDefault();

                    if (photo != null)
                    {
                        var fileId = photo.FileId;
                        var file = await _botClient.GetFileAsync(fileId);

                        var fileName = $"{file.FileId}_{file.FilePath.Split('/').Last()}";
                        var filePath = Path.Combine(photosPath, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await _botClient.DownloadFileAsync(file.FilePath, fileStream);
                        }

                        photos.Add(fileName);
                    }
                }
            }

            // Передаємо список текстів повідомлень та фото у відображення
            var viewModel = new NewsModel
            {
                Messages = messages,
                Photos = photos
            };
            return View(viewModel);
        }

        



    }
}
