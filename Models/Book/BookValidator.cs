namespace MvcTask.Models.Book;

public class BookValidator : AbstractValidator<Book>
{
    private const int MINIMUM_PUBLISH_YEAR = 1000;

    public BookValidator()
    {
        // TODO: finish this
        RuleFor(book => book.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(book => book.Author).NotEmpty().WithMessage("Author is required.");
        RuleFor(book => book.YearPublished).InclusiveBetween(MINIMUM_PUBLISH_YEAR, DateTime.Now.Year)
            .WithMessage("Year published is invalid.");
        RuleFor(book => book.Genre).IsInEnum().WithMessage("Genre is invalid.");
    }
}