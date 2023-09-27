namespace MvcTask.Models.Book;

public class BookValidator : AbstractValidator<Book>
{
    private const int MINIMUM_PUBLISH_YEAR = 1000;
    private const int ISBN13_LENGTH = 13;
    private const int ISBN_LENGTH = 10;
    private const string ONLY_NUMBERS_REGEX = "^[0-9]*$";
    private const string VALID_ISBN_REGEX = "^[0-9]{9}[0-9X]$";

    private static readonly string[] ValidIsbn13Prefixes =
    {
        "978", "979"
    };

    public BookValidator()
    {
        // Basic validation
        RuleFor(book => book.Title)
            .NotEmpty()
            .WithMessage("Title is required.");
        RuleFor(book => book.Author)
            .NotEmpty()
            .WithMessage("Author is required.");
        RuleFor(book => book.YearPublished)
            .InclusiveBetween(MINIMUM_PUBLISH_YEAR, DateTime.Now.Year)
            .WithMessage("Year published is invalid.");
        RuleFor(book => book.Genre)
            .IsInEnum()
            .WithMessage("Genre is invalid.");

        // ISBN validation
        RuleFor(book => new { ISBN = book.Isbn, ISBN13 = book.Isbn13 })
            .Must(values => !string.IsNullOrWhiteSpace(values.ISBN) || !string.IsNullOrWhiteSpace(values.ISBN13))
            .WithMessage("ISBN13 or ISBN must be provided.");

        // ISBN10 validation
        When(book => !string.IsNullOrWhiteSpace(book.Isbn), () =>
        {
            RuleFor(book => book.Isbn)
                .NotEmpty()
                .Length(ISBN_LENGTH)
                .Matches(VALID_ISBN_REGEX)
                .WithMessage($"ISBN should be a valid {ISBN_LENGTH} number string.");
        });

        // ISBN13 validation
        When(book => !string.IsNullOrWhiteSpace(book.Isbn13), () =>
        {
            RuleFor(book => book.Isbn13)
                .NotEmpty()
                .Length(ISBN13_LENGTH)
                .Matches(ONLY_NUMBERS_REGEX)
                .WithMessage($"ISBN13 should be a valid {ISBN13_LENGTH} number string.")
                .Must(isbn13 => ValidIsbn13Prefixes.Contains(isbn13[..3]))
                .WithMessage(
                    $"ISBN13 should start with any of the valid prefixes {string.Join(", ", ValidIsbn13Prefixes)}");
        });
    }
}