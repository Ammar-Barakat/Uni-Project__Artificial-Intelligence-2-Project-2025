using bookhouseAPI.Data;
using bookhouseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace bookhouseAPI.Repository.BookRepository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _client;
        public BookRepository(ApplicationDbContext context, HttpClient client)
        {
            _context = context;
            _client = client;
        }

        public async Task<Book> GetBookById(int Id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == Id);
            return book;
        }

        public async Task<Book> GetBookByTitle(string title)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Title == title);
            return book;
        }

        public async Task<Book> Details(int Id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == Id);

            return book;
        }

        public async Task<List<Book>> Recommendations(int Id)
        {
            var book = await GetBookById(Id);
            var bookTitles = await _client.GetFromJsonAsync<List<string>>($"http://127.0.0.1:5000/get-recommendations/{book.Title}/{book.Author}/{book.Genres}");

            string pattern = @"^(.*?)\s*\(";

            List<Book> recommendations = new List<Book>();

            foreach (var bookTitle in bookTitles)
            {
                Match match = Regex.Match(bookTitle, pattern);

                if (match.Success)
                {
                    string extractedTitle = match.Groups[1].Value;
                    var details = await GetBookByTitle(extractedTitle);

                    if (details != null)
                    {

                        Book recommendation = new Book
                        {
                            Id = details.Id,
                            Title = details.Title,
                            Cover = details.Cover
                        };

                        recommendations.Add(recommendation);
                    }
                }
                else
                {
                    var details = await GetBookByTitle(bookTitle);

                    if (details != null)
                    {

                        Book recommendation = new Book
                        {
                            Id = details.Id,
                            Title = details.Title,
                            Cover = details.Cover
                        };

                        recommendations.Add(recommendation);
                    }
                }
            }

            //foreach (var recommendation in recommendations)
            //{
            //    if (recommendation.Title == book.Title)
            //    {
            //        recommendations.Remove(recommendation);
            //    }
            //}

            return recommendations;

            //var title = await GetBookById(Id);
            //var booksId = await _client.GetFromJsonAsync<List<string>>($"http://127.0.0.1:5000/get-recommendations/{title}");

            //List<Book> recommendations = new List<Book>();

            //foreach(var bookId in booksId)
            //{
            //    Match match = Regex.Match(bookId, @"^\d+");
            //    if (match.Success)
            //    {
            //        var book = await Details(int.Parse(match.Value));
            //        Book recommendation = new Book
            //        {
            //            Id = book.Id,
            //            Title  = book.Title,
            //            Author = book.Author,
            //            Cover = book.Cover
            //        };

            //        recommendations.Add(recommendation);
            //    }
            //}

            //return recommendations;
        }

        public async Task<List<Book>> Search(string query)
        {
            List<Book> books = new List<Book>();

            var firstBooks = await _context.Books
                  .Where(b => b.Title.Equals(query.ToLower()))
                  .OrderByDescending(b => b.Rating)
                  .ToListAsync();

            var secondBooks = await _context.Books
                .Where(b => b.Title.ToLower().Contains(query.ToLower()))
                .OrderByDescending(book => book.Rating)
                .ToListAsync();

            books.AddRange(firstBooks);

            foreach (var book in secondBooks)
            {
                if (!books.Contains(book))
                {
                    books.Add(book);
                }
            }

            return books;
        }
    }
}
