using LinkedNewsChatApp.Migrations;
using ServiceLayer;
using LinkedNewsChatApp.Data;
namespace LinkedNewsChatApp.Data
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NewsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public byte[] GetNewsPhoto(int id) {
            byte[] photo = _dbContext.news.Where(m => m.id == id).Select(k => k.photo).FirstOrDefault();
            return photo;
        }
    }
}
