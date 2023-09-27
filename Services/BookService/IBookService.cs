namespace MvcTask.Services.BookService;

public interface IBookService
{
    public IEnumerable<Book> GetBooks();
    public Book? GetBook(string id);
    public void CreateBook(Book book);
    public void UpdateBook(string id, BookDto book);
    public void DeleteBook(string id);
}