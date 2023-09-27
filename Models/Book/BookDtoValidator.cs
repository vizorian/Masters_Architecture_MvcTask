namespace MvcTask.Models.Book;

public class BookDtoValidator : AbstractValidator<BookDto>
{
    private const int MINIMUM_PUBLISH_YEAR = 1400;

    public BookDtoValidator()
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
    }
}