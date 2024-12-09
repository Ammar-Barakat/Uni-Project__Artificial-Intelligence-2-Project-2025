using bookhouseAPI.Models;

namespace bookhouseAPI.Repository.BookRepository
{
    public interface IBookRepository
    {
        Task<List<Book>> Search(string query);
        Task<Book> Details(int Id);
        Task<List<Book>> Recommendations(int Id);
    }
}
