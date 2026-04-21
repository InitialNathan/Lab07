using Xunit;
using BookLab5.Services;
using BookLab5.Models;
using System.Linq;

namespace BookLab5.Tests
{
    public class LibraryServiceTests
    {
        //  BOOK TESTS 

        [Fact]
        public void AddBook_ShouldIncreaseCount()
        {
            // Arrange ///Edit new LibraryService(False); to empty
            var service = new LibraryService();
            int initialCount = service.GetBooks().Count;

            var book = new Book
            {
                Title = "Test Book",
                Author = "Tester",
                ISBN = "123"
            };

            // Act
            service.AddBook(book);

            // Assert
            Assert.Equal(initialCount + 1, service.GetBooks().Count);
        }

        [Fact]
        public void DeleteBook_ShouldRemoveBook()
        {
            // Arrange
            var service = new LibraryService(false);

            var book = new Book
            {
                Title = "Delete Me",
                Author = "Test",
                ISBN = "999"
            };

            service.AddBook(book);
            var addedBook = service.GetBooks().Last();

            // Act
            service.DeleteBook(addedBook.Id);

            // Assert
            Assert.DoesNotContain(service.GetBooks(), b => b.Id == addedBook.Id);
        }

        [Fact]
        public void UpdateBook_ShouldModifyBook()
        {
            // Arrange
            var service = new LibraryService(false);

            var book = new Book
            {
                Title = "Original",
                Author = "Author",
                ISBN = "111"
            };

            service.AddBook(book);
            var addedBook = service.GetBooks().Last();

            addedBook.Title = "Updated";

            // Act
            service.UpdateBook(addedBook);

            // Assert
            var updated = service.GetBooks().First(b => b.Id == addedBook.Id);
            Assert.Equal("Updated", updated.Title);
        }

        [Fact]
        public void DeleteBook_InvalidId_ShouldDoNothing()
        {
            // Arrange
            var service = new LibraryService(false);
            int countBefore = service.GetBooks().Count;

            // Act
            service.DeleteBook(9999);

            // Assert
            Assert.Equal(countBefore, service.GetBooks().Count);
        }

        //  USER TESTS 

        [Fact]
        public void AddUser_ShouldIncreaseCount()
        {
            // Arrange
            var service = new LibraryService(false);
            int initialCount = service.GetUsers().Count;

            var user = new User
            {
                Name = "Test User",
                Email = "test@test.com"
            };

            // Act
            service.AddUser(user);

            // Assert
            Assert.Equal(initialCount + 1, service.GetUsers().Count);
        }

        [Fact]
        public void DeleteUser_ShouldRemoveUser()
        {
            // Arrange
            var service = new LibraryService(false);

            var user = new User
            {
                Name = "Delete User",
                Email = "delete@test.com"
            };

            service.AddUser(user);
            var addedUser = service.GetUsers().Last();

            // Act
            service.DeleteUser(addedUser.Id);

            // Assert
            Assert.DoesNotContain(service.GetUsers(), u => u.Id == addedUser.Id);
        }

        //  BORROW / RETURN 

        [Fact]
        public void BorrowBook_ShouldRemoveBookFromAvailable()
        {
            // Arrange
            var service = new LibraryService(false);

            var user = new User
            {
                Name = "Borrower",
                Email = "borrow@test.com"
            };
            service.AddUser(user);
            var addedUser = service.GetUsers().Last();

            var book = new Book
            {
                Title = "Borrow Me",
                Author = "A",
                ISBN = "222"
            };
            service.AddBook(book);
            var addedBook = service.GetBooks().Last();

            // Act
            service.BorrowBook(addedUser.Id, addedBook.Id);

            // Assert
            Assert.DoesNotContain(service.GetBooks(), b => b.Id == addedBook.Id);
        }

        [Fact]
        public void ReturnBook_ShouldAddBookBack()
        {
            // Arrange
            var service = new LibraryService(false);

            var user = new User
            {
                Name = "Returner",
                Email = "return@test.com"
            };
            service.AddUser(user);
            var addedUser = service.GetUsers().Last();

            var book = new Book
            {
                Title = "Return Me",
                Author = "B",
                ISBN = "333"
            };
            service.AddBook(book);
            var addedBook = service.GetBooks().Last();

            service.BorrowBook(addedUser.Id, addedBook.Id);

            // Act
            service.ReturnBook(addedUser.Id, addedBook.Id);

            // Assert
            Assert.Contains(service.GetBooks(), b => b.Id == addedBook.Id);
        }

        [Fact]
        public void BorrowBook_InvalidBook_ShouldNotCrash()
        {
            // Arrange
            var service = new LibraryService(false);

            var user = new User
            {
                Name = "Test",
                Email = "test@test.com"
            };
            service.AddUser(user);
            var addedUser = service.GetUsers().Last();

            // Act
            service.BorrowBook(addedUser.Id, 9999);

            // Assert
            Assert.True(true); // passes if no exception
        }
    }
}