using bookhouseAPI.Repository.BookRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bookhouseAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var books = await _bookRepository.Search(query);

            if (books == null)
                return NotFound("No Results were Found!");

            return Ok(books);
        }

        [HttpGet("details/{Id}")]
        public async Task<IActionResult> Details(int Id)
        {
            var book = await _bookRepository.Details(Id);

            if (book == null)
                return NotFound("No Results were Found!");

            return Ok(book);
        }

        [HttpGet("recommendations/{Id}")]
        public async Task<IActionResult> Recommendations(int Id)
        {
            var recommendations = await _bookRepository.Recommendations(Id);

            if (recommendations == null)
                return NotFound("No Results were Found!");

            return Ok(recommendations);
        }
    }
}
