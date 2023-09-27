namespace MvcTask.Validators;

public class IsbnValidator : AbstractValidator<string>
{
    private const int ISBN13_LENGTH = 13;
    private const int ISBN10_LENGTH = 10;
    private const string ONLY_NUMBERS_REGEX = "^[0-9]*$";
    private const string VALID_ISBN10_REGEX = "^[0-9]{9}[0-9X]$";

    private static readonly string[] ValidIsbn13Prefixes =
    {
        "978", "979"
    };

    public IsbnValidator()
    {
        // length validation
        RuleFor(isbn => isbn)
            .NotEmpty()
            .WithMessage("ISBN is required.")
            .Must(isbn => isbn.Length is ISBN13_LENGTH or ISBN10_LENGTH)
            .WithMessage("ISBN should be of valid length.");

        // ISBN13 validation
        When(isbn => isbn.Length is ISBN13_LENGTH, () =>
        {
            RuleFor(isbn => isbn)
                .Matches(ONLY_NUMBERS_REGEX)
                .WithMessage($"ISBN13 should be a valid {ISBN13_LENGTH} number string.")
                .Must(isbn13 => ValidIsbn13Prefixes.Contains(isbn13[..3]))
                .WithMessage($"ISBN13 should start with any of the valid prefixes {string.Join(", ", ValidIsbn13Prefixes)}");
        });

        // ISBN10 validation
        When(isbn => isbn.Length is ISBN10_LENGTH, () =>
        {
            RuleFor(isbn => isbn)
                .Matches(VALID_ISBN10_REGEX)
                .WithMessage($"ISBN should be a valid {ISBN10_LENGTH} character string and can only end with digits or X.");
        });
    }
}