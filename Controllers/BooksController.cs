namespace MvcTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IsbnValidator _isbnValidator = new();
    private readonly BookValidator _bookValidator = new();
    private readonly BookDtoValidator _bookDtoValidator = new();

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Gets all books.
    /// </summary>
    /// <response code="200">All books.</response>
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetBooks()
    {
        var books = _bookService.GetBooks();
        return Ok(books);
    }

    /// <summary>
    /// Gets a book by ID.
    /// </summary>
    /// <param name="id">ISBN13 or ISBN</param>
    /// <response code="200">Book with the specified ID.</response>
    /// <response code="404">Book with the specified ID was not found.</response>
    [HttpGet("{id}")]
    public ActionResult<Book> GetBook(string id)
    {
        var isbnValidationResult = _isbnValidator.Validate(id);

        if (!isbnValidationResult.IsValid)
        {
            foreach (var validationFailure in isbnValidationResult.Errors)
            {
                ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }

            return BadRequest(ModelState);
        }

        var book = _bookService.GetBook(id);
        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    /// <summary>
    /// Creates a new book.
    /// </summary>
    /// <param name="book"></param>
    /// <response code="201">Book was created.</response>
    /// <response code="400">Parameter validation failed.</response>
    [HttpPost]
    public ActionResult<Book> CreateBook(Book book)
    {
        var bookValidationResult = _bookValidator.Validate(book);

        if (!bookValidationResult.IsValid)
        {
            foreach (var validationFailure in bookValidationResult.Errors)
            {
                ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }

            return BadRequest(ModelState);
        }

        _bookService.CreateBook(book);
        return Created($"/books/{book.Id}", book);
    }

    /// <summary>
    /// Deletes a book by ID.
    /// </summary>
    /// <param name="id">ISBN13 or ISBN</param>
    /// <response code="204">Book was deleted.</response>
    /// <response code="400">Parameter validation failed.</response>
    [HttpDelete("{id}")]
    public IActionResult DeleteBook(string id)
    {
        var isbnValidationResult = _isbnValidator.Validate(id);

        if (!isbnValidationResult.IsValid)
        {
            foreach (var validationFailure in isbnValidationResult.Errors)
            {
                ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }

            return BadRequest(ModelState);
        }

        _bookService.DeleteBook(id);
        return NoContent();
    }

    /// <summary>
    /// Updates a book by ID.
    /// </summary>
    /// <param name="id">ISBN13 or ISBN</param>
    /// <param name="book"></param>
    /// <response code="204">Book was updated.</response>
    /// <response code="400">Parameter validation failed.</response>
    [HttpPut("{id}")]
    public IActionResult UpdateBook(string id, BookDto book)
    {
        var isbnValidationResult = _isbnValidator.Validate(id);

        if (!isbnValidationResult.IsValid)
        {
            foreach (var validationFailure in isbnValidationResult.Errors)
            {
                ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }

            return BadRequest(ModelState);
        }

        var bookDtoValidationResult = _bookDtoValidator.Validate(book);

        if (!bookDtoValidationResult.IsValid)
        {
            foreach (var validationFailure in bookDtoValidationResult.Errors)
            {
                ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }

            return BadRequest(ModelState);
        }

        _bookService.UpdateBook(id, book);
        return NoContent();
    }
}