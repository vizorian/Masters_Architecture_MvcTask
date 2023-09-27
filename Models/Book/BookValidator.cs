namespace MvcTask.Models.Book;

public class BookValidator : AbstractValidator<Book>
{
    private const int MINIMUM_PUBLISH_YEAR = 1400;

    public BookValidator()
    {
        RuleFor(book => book.Title)
            .NotEmpty()
            .WithMessage("Title is required.");
        RuleFor(book => book.Author)
            .NotEmpty()
            .WithMessage("Author is required.");
        RuleFor(book => book.YearPublished)
            .InclusiveBetween(MINIMUM_PUBLISH_YEAR, DateTime.Now.Year)
            .WithMessage($"Year published is invalid. Must be in range {MINIMUM_PUBLISH_YEAR}-{DateTime.Now.Year}");
        RuleFor(book => book.Genre)
            .IsInEnum()
            .WithMessage("Genre is invalid.");

        // ISBN validation
        RuleFor(book => new { ISBN = book.Isbn, ISBN13 = book.Isbn13 })
            .Must(values => !string.IsNullOrWhiteSpace(values.ISBN) || !string.IsNullOrWhiteSpace(values.ISBN13))
            .WithMessage("At least one of ISBN13 or ISBN is required.");

        RuleFor(book => book.Isbn13)
            .SetValidator(new IsbnValidator())
            .When(book => !string.IsNullOrWhiteSpace(book.Isbn13));
        RuleFor(book => book.Isbn)
            .SetValidator(new IsbnValidator())
            .When(book => !string.IsNullOrWhiteSpace(book.Isbn));
    }
}