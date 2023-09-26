namespace MvcTask.Models.Book;

public class BookValidator : AbstractValidator<Book>
{
    private const int MINIMUM_PUBLISH_YEAR = 1000;
    private const int ISBN13_LENGTH = 13;
    private const int ISBN_LENGTH = 10;
    private const string ONLY_NUMBERS_REGEX = "^[0-9]*$";

    public BookValidator()
    {
        RuleFor(book => book.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(book => book.Author).NotEmpty().WithMessage("Author is required.");
        RuleFor(book => book.YearPublished).InclusiveBetween(MINIMUM_PUBLISH_YEAR, DateTime.Now.Year)
            .WithMessage("Year published is invalid.");
        RuleFor(book => book.Genre).IsInEnum().WithMessage("Genre is invalid.");

        RuleFor(book => book.ISBN13).NotEmpty().Unless(book => string.IsNullOrWhiteSpace(book.ISBN))
            .WithMessage("ISBN13 or ISBN must be provided.");
        RuleFor(book => book.ISBN).NotEmpty().Unless(book => string.IsNullOrWhiteSpace(book.ISBN13))
            .WithMessage("ISBN13 or ISBN must be provided.");

        When(book => !string.IsNullOrWhiteSpace(book.ISBN), () =>
        {
            RuleFor(book => book.ISBN).NotEmpty().Length(ISBN_LENGTH).Matches(ONLY_NUMBERS_REGEX)
                .WithMessage("ISBN should be a valid 10 number string");
        });

        When(book => !string.IsNullOrWhiteSpace(book.ISBN13), () =>
        {
            RuleFor(book => book.ISBN).NotEmpty().Length(ISBN13_LENGTH).Matches(ONLY_NUMBERS_REGEX)
                .WithMessage("ISBN13 should be a valid 13 number string");
        });
    }
}