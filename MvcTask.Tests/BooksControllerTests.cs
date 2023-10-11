namespace MvcTask.Tests;

public class BooksControllerTests
{
    private readonly Mock<IBookRepository> _bookServiceMock = new();

    private const string FAKE_BOOK_GOOD_ID = "9780000000000";
    private const string FAKE_BOOK_BAD_ID = "123";

    private static readonly Book GoodBook = new("9783161484100")
    {
        Author = "J. K. Rowling",
        Title = "Harry Potter and the Philosopher's Stone",
        YearPublished = 1997,
        Description =
            "Harry Potter and the Philosopher's Stone is a fantasy novel written by British author J. K. Rowling. The first novel in the Harry Potter series and Rowling's debut novel, it follows Harry Potter, a young wizard who discovers his magical heritage on his eleventh birthday, when he receives a letter of acceptance to Hogwarts School of Witchcraft and Wizardry.",
        Genre = Genre.Fantasy,
    };

    private readonly List<Book> _goodBooks = new() { GoodBook };

    private static readonly Book BadBook = new("412415")
    {
        Author = "Trash man",
        Title = "Trash",
        YearPublished = 30000,
        Description = string.Empty,
        Genre = Genre.Horror,
    };

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
        _bookServiceMock.Setup(service => service.GetBooks()).Returns(_goodBooks);
        _bookServiceMock.Setup(service => service.GetBook(GoodBook.Id)).Returns(GoodBook);
    }

    [Test]
    public void GetBooks_ReturnsOk()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.GetBooks();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public void GetBooks_ReturnsAllBooks()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.GetBooks();

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(_goodBooks);
    }

    [Test]
    public void GetBook_ReturnsOk_WhenBookExists()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.GetBook(GoodBook.Id);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Test]
    public void GetBook_ReturnsBook_WhenBookExists()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.GetBook(GoodBook.Id);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(GoodBook);
    }

    [Test]
    public void GetBook_ReturnsNotFound_WhenBookDoesNotExist()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.GetBook(FAKE_BOOK_GOOD_ID);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public void GetBook_ReturnsBadRequest_WhenIdValidationFails()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.GetBook(FAKE_BOOK_BAD_ID);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public void CreateBook_ReturnsCreated_WhenBookIsCreated()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.CreateBook(GoodBook);

        // Assert
        result.Should().BeOfType<CreatedResult>();
    }

    [Test]
    public void CreateBook_ReturnsBadRequest_WhenBookAlreadyExists()
    {
        // Arrange
        var bookServiceMock = new Mock<IBookRepository>();
        bookServiceMock.Setup(service => service.CreateBook(GoodBook))
            .Throws<DuplicateNameException>();
        var controller = new BooksController(bookServiceMock.Object);

        // Act
        var createBook = () => controller.CreateBook(GoodBook);

        // Assert
        createBook.Should().Throw<DuplicateNameException>();
    }

    [Test]
    public void CreateBook_ReturnsBadRequest_WhenBookValidationFails()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.CreateBook(BadBook);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public void UpdateBook_ReturnsNoContent_WhenBookIsUpdated()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.UpdateBook(GoodBook.Id, GoodBookDto);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public void UpdateBook_ReturnsBadRequest_WhenBookDtoValidationFails()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.UpdateBook(FAKE_BOOK_GOOD_ID, new BookDto());

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public void UpdateBook_ReturnsBadRequest_WhenBookDoesNotExist()
    {
        // Arrange
        var bookServiceMock = new Mock<IBookRepository>();
        bookServiceMock.Setup(service => service.UpdateBook(FAKE_BOOK_GOOD_ID, GoodBookDto))
            .Throws<ServiceException>();
        var controller = new BooksController(bookServiceMock.Object);

        // Act
        var updateBook = () => controller.UpdateBook(FAKE_BOOK_GOOD_ID, GoodBookDto);

        // Assert
        updateBook.Should().Throw<ServiceException>();
    }

    [Test]
    public void UpdateBook_ReturnsBadRequest_WhenIdValidationFails()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.UpdateBook(FAKE_BOOK_BAD_ID, GoodBookDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public void DeleteBook_ReturnsNoContent_WhenBookIsDeleted()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.DeleteBook(GoodBook.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public void DeleteBook_ReturnsBadRequest_WhenIdValidationFails()
    {
        // Arrange
        var controller = new BooksController(_bookServiceMock.Object);

        // Act
        var result = controller.DeleteBook(FAKE_BOOK_BAD_ID);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Test]
    public void DeleteBook_ThrowsException_WhenBookDoesNotExist()
    {
        // Arrange
        var bookServiceMock = new Mock<IBookRepository>();
        bookServiceMock.Setup(service => service.DeleteBook(FAKE_BOOK_GOOD_ID))
            .Throws<ServiceException>();
        var controller = new BooksController(bookServiceMock.Object);

        // Act
        var deleteBook = () => controller.DeleteBook(FAKE_BOOK_GOOD_ID);

        // Assert
        deleteBook.Should().Throw<ServiceException>();
    }
}