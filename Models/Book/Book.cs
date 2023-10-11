namespace MvcTask.Models.Book;

[PrimaryKey(nameof(Id))]
public class Book
{
    public Book(string? isbn13 = null!, string? isbn = null!)
    {
        Isbn13 = isbn13;
        Isbn = isbn;
        Id = DetermineIdFromIsbn();
    }

    public string Id { get; private set; }
    [BindNever] public string? Isbn13 { get; set; }
    [BindNever] public string? Isbn { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int YearPublished { get; set; }
    public string Description { get; set; } = string.Empty;
    public Genre Genre { get; set; }

    private string DetermineIdFromIsbn()
    {
        if (Isbn13 is null && Isbn is null)
        {
            throw new ServiceException("At least one of ISBN13 or ISBN is required.");
        }

        return !string.IsNullOrWhiteSpace(Isbn13)
            ? Isbn13
            : Isbn!;
    }
}