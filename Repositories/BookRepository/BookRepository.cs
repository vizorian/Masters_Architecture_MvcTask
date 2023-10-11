namespace MvcTask.Repositories.BookRepository;

public class BookRepository : IBookRepository
{
    private const int ISBN_LENGTH = 10;

    private readonly BookDbContext _dbContext;

    public BookRepository(BookDbContext dbContext)
    {
        _dbContext = dbContext;
        if (!_dbContext.Books.Any())
        {
            SeedDummyData();
        }
    }

    private void SeedDummyData()
    {
        _dbContext.Books.AddRange(_dummyBooks);
        _dbContext.SaveChanges();
    }

    public IEnumerable<Book> GetBooks()
    {
        return _dbContext.Books.ToList();
    }

    public Book? GetBook(string id)
    {
        return id.Length == ISBN_LENGTH
            ? _dbContext.Books.FirstOrDefault(book => book.Isbn == id)
            : _dbContext.Books.FirstOrDefault(book => book.Isbn13 == id);
    }

    public void CreateBook(Book book)
    {
        if (_dbContext.Books.Any(existingBook => existingBook.Id == book.Id))
        {
            throw new DuplicateNameException($"Attempted to create book with duplicate ID {book.Id}.");
        }

        _dbContext.Books.Add(book);
        _dbContext.SaveChanges();
    }

    public void UpdateBook(string id, BookDto book)
    {
        var existingBook = GetBook(id);

        if (existingBook is null)
        {
            throw new ServiceException($"Failed to find book with ID {id}");
        }

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.YearPublished = book.YearPublished;
        existingBook.Description = book.Description;
        existingBook.Genre = book.Genre;
        _dbContext.SaveChanges();
    }

    public void DeleteBook(string id)
    {
        var existingBook = GetBook(id);

        if (existingBook is null)
        {
            throw new ServiceException($"Failed to find book with ID {id}");
        }

        _dbContext.Books.Remove(existingBook);
        _dbContext.SaveChanges();
    }

    private readonly HashSet<Book> _dummyBooks = new()
    {
        new Book("9780756405892", "9756405890")
        {
            Title = "The Name of the Wind",
            Author = "Patrick Rothfuss",
            YearPublished = 2009,
            Genre = Genre.Fantasy
        },
        new Book("9780553573404", "0553573403")
        {
            Title = "A Game of Thrones",
            Author = "George R.R. Martin",
            YearPublished = 1997,
            Genre = Genre.Fantasy
        },
        new Book("9780441020676")
        {
            Title = "Those Across the River",
            Author = "Christopher Buehlman",
            YearPublished = 2011,
            Genre = Genre.Horror
        }
    };
}