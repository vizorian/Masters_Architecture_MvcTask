namespace MvcTask.Services.BookService;

public interface IBookService
{
    public IEnumerable<Book> GetBooks();
    public Book? GetBook(string id);
    public void CreateBook(Book book);
    public void UpdateBook(string id, Book book);
    public void DeleteBook(string id);
}