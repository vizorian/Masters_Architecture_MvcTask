namespace MvcTask.Tests;

[TestFixture]
public class BookTests
{
    [Test]
    public void NewBookId_ThrowsException_WhenAllIsbnIsNull()
    {
        // Arrange
        var createBook = () => new Book
        {
            Isbn = null,
            Isbn13 = null
        };
        
        // Act & Assert
        createBook.Should().Throw<ServiceException>();
    }
}