namespace MvcTask.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetBooks()
    {
        var books = _bookService.GetBooks();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetBook(string id)
    {
        var book = _bookService.GetBook(id);
        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    public ActionResult<Book> CreateBook(Book book)
    {
        var validator = new BookValidator();
        var validationResult = validator.Validate(book);

        if (!validationResult.IsValid)
        {
            foreach (var validationFailure in validationResult.Errors)
            {
                ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }

            return BadRequest(book);
        }

        _bookService.CreateBook(book);
        return CreatedAtAction(nameof(GetBook), GetBook(book.Id));
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("ID is required");
        }

        _bookService.DeleteBook(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBook(string id, Book book)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("ID is required");
        }

        var validator = new BookValidator();
        var validationResult = validator.Validate(book);

        if (!validationResult.IsValid)
        {
            foreach (var validationFailure in validationResult.Errors)
            {
                ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }

            return BadRequest(book);
        }

        _bookService.UpdateBook(id, book);
        return NoContent();
    }
}