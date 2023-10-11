namespace MvcTask.Repositories.BookRepository;

public interface IBookRepository
{
    public IEnumerable<Book> GetBooks();
    public Book? GetBook(string id);
    public void CreateBook(Book book);
    public void UpdateBook(string id, BookDto book);
    public void DeleteBook(string id);
}