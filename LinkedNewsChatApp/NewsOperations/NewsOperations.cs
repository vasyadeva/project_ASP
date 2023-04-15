using DataLayer;
using LinkedNewsChatApp.Data;
using ServiceLayer;

namespace LinkedNewsChatApp.NewsOperations
{
    public class NewsOperations
    {
        private readonly NewsRepository _repository;
        

        public NewsOperations(NewsRepository repository)
        {
            _repository = repository;
        }
        public byte[] GetNewsPhoto(int id) {
            return _repository.GetNewsPhoto(id);
        }
    }
}
