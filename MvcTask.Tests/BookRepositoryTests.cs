namespace MvcTask.Tests;

public class BookRepositoryTests
{
    private readonly DbContextOptions<BookDbContext> _dbContextOptions = new DbContextOptionsBuilder<BookDbContext>()
        .UseInMemoryDatabase(databaseName: "MvcTaskTests")
        .Options;

    private readonly Mock<BookDbContext> _dbContextMock = new();

    private const string FAKE_BOOK_GOOD_ID = "9780000000000";

    private static readonly Book GoodBook = new("9783161484100", "3161484100")
    {
        Author = "J. K. Rowling",
        Title = "Harry Potter and the Philosopher's Stone",
        YearPublished = 1997,
        Description = "Some Description",
        Genre = Genre.Fantasy,
    };

    private static readonly Book GoodBook2 = new("9784567484100", "4567484100")
    {
        Author = "J. K. Rowling",
        Title = "Harry Potter and the Chamber of Secrets",
        YearPublished = 1998,
        Description = "Some Description",
        Genre = Genre.Fantasy,
    };

    private readonly List<Book> _goodBooks = new() { GoodBook };

    private static readonly BookDto GoodBookDto = new()
    {
        Title = "Some Title",
        Author = "Some Author",
        YearPublished = DateTime.Now.Year,
        Description = "Some Description",
        Genre = Genre.Fantasy
    };

    [SetUp]
    public void Setup()
    {
        _dbContextMock.Setup(db => db.Books).ReturnsDbSet(_goodBooks);
    }

    [Test]
    public void GetBooks_ReturnsAllBooks()
    {
        // Arrange
        var repository = new BookRepository(_dbContextMock.Object);

        // Act
        var result = repository.GetBooks();

        // Assert
        result.Should().BeEquivalentTo(_goodBooks);
    }

    [Test]
    public void GetBook_ReturnsBook()
    {
        // Arrange
        var repository = new BookRepository(_dbContextMock.Object);

        // Act
        var result = repository.GetBook(GoodBook.Id);

        // Assert
        result.Should().BeEquivalentTo(GoodBook);
    }

    [Test]
    public void GetBook_ReturnsNull_WhenBookIsNotFound()
    {
        // Arrange
        var repository = new BookRepository(_dbContextMock.Object);

        // Act
        var result = repository.GetBook(FAKE_BOOK_GOOD_ID);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void CreateBook_AddsBook()
    {
        // Arrange
        var dbContextMock = new Mock<BookDbContext>();
        dbContextMock.Setup(db => db.Books).ReturnsDbSet(new List<Book> { GoodBook2 });
        var repository = new BookRepository(dbContextMock.Object);

        // Act
        repository.CreateBook(GoodBook);

        // Assert
        dbContextMock.Verify(db => db.SaveChanges(), Times.Once);
    }

    [Test]
    public void CreateBook_ThrowsDuplicateNameException_WhenDuplicateBookIdExists()
    {
        // Arrange
        var dbContextMock = new Mock<BookDbContext>();
        dbContextMock.Setup(db => db.Books).ReturnsDbSet(_goodBooks);
        var repository = new BookRepository(dbContextMock.Object);

        // Act
        var createBook = () => repository.CreateBook(GoodBook);

        // Assert
        createBook.Should().Throw<DuplicateNameException>();
    }

    [Test]
    public void UpdateBook_UpdatesBook()
    {
        // Arrange
        var dbContextMock = new Mock<BookDbContext>();
        dbContextMock.Setup(db => db.Books).ReturnsDbSet(_goodBooks);
        var repository = new BookRepository(dbContextMock.Object);

        // Act
        repository.UpdateBook(GoodBook.Id, GoodBookDto);

        // Assert
        dbContextMock.Verify(db => db.SaveChanges(), Times.Once);
    }

    [Test]
    public void UpdateBook_ThrowsServiceException_WhenBookIsNotFound()
    {
        // Arrange
        var repository = new BookRepository(_dbContextMock.Object);

        // Act
        var updateBook = () => repository.UpdateBook(FAKE_BOOK_GOOD_ID, GoodBookDto);

        // Assert
        updateBook.Should().Throw<ServiceException>();
    }

    [Test]
    public void DeleteBook_DeletesBook()
    {
        // Arrange
        var dbContextMock = new Mock<BookDbContext>();
        dbContextMock.Setup(db => db.Books).ReturnsDbSet(_goodBooks);
        var repository = new BookRepository(dbContextMock.Object);

        // Act
        repository.DeleteBook(GoodBook.Id);

        // Assert
        dbContextMock.Verify(db => db.SaveChanges(), Times.Once);
    }

    [Test]
    public void DeleteBook_ThrowsServiceException_WhenBookIsNotFound()
    {
        // Arrange
        var repository = new BookRepository(_dbContextMock.Object);

        // Act
        var deleteBook = () => repository.DeleteBook(FAKE_BOOK_GOOD_ID);

        // Assert
        deleteBook.Should().Throw<ServiceException>();
    }

    [Test]
    public void GetBook_ReturnsBook_WhenBookIdIsIsbn13()
    {
        // Arrange
        var repository = new BookRepository(_dbContextMock.Object);

        // Act
        var result = repository.GetBook(GoodBook.Isbn13!);

        // Assert
        result.Should().BeEquivalentTo(GoodBook);
    }

    [Test]
    public void GetBook_ReturnsBook_WhenBookIdIsIsbn10()
    {
        // Arrange
        var repository = new BookRepository(_dbContextMock.Object);

        // Act
        var result = repository.GetBook(GoodBook.Isbn!);

        // Assert
        result.Should().BeEquivalentTo(GoodBook);
    }
}