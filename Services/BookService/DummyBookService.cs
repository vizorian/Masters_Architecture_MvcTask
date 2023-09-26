namespace MvcTask.Services.BookService;

public class DummyBookService : IBookService
{
    private readonly HashSet<Book> _books = new()
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

    public IEnumerable<Book> GetBooks()
    {
        return _books;
    }

    public Book? GetBook(string id)
    {
        return _books.FirstOrDefault(book => book.Id == id);
    }

    public void CreateBook(Book book)
    {
        if (_books.Any(existingBook => existingBook.Id == book.Id))
        {
            throw new DuplicateNameException($"Attempted to create book with duplicate ID {book.Id}.");
        }

        _books.Add(book);
    }

    public void UpdateBook(string id, Book book)
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
    }

    public void DeleteBook(string id)
    {
        var existingBook = GetBook(id);

        if (existingBook is null)
        {
            throw new ServiceException($"Failed to find book with ID {id}");
        }

        _books.Remove(existingBook);
    }
}