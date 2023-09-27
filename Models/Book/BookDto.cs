// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace MvcTask.Models.Book;

public class BookDto
{
    public string Title { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public int YearPublished { get; init; }
    public string Description { get; init; } = string.Empty;
    public Genre Genre { get; init; }
}