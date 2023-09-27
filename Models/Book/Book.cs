namespace MvcTask.Models.Book;

public class Book
{
    private const string DEFAULT_ISBN_VALUE = "";

    public Book(string isbn13 = DEFAULT_ISBN_VALUE, string isbn = DEFAULT_ISBN_VALUE)
    {
        var validator = new BookValidator();
        var validationResult = validator.Validate(this);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        Isbn13 = isbn13;
        Isbn = isbn;
    }

    public string Id => !string.IsNullOrWhiteSpace(Isbn13) ? Isbn13 : Isbn;
    public string Isbn13 { get; }
    public string Isbn { get; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int YearPublished { get; set; }
    public string Description { get; set; } = string.Empty;
    public Genre Genre { get; set; }
}