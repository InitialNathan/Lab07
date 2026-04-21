using BookLab5.Models;
namespace BookLab5.Services

{
    public interface ILibraryService
    {
        List<Book> GetBooks();
        List<User> GetUsers();

        void AddBook(Book book);
        void EditBook(Book book);
        void DeleteBook(int id);

        void AddUser(User user);
        void EditUser(User user);
        void DeleteUser(int id);

        void BorrowBook(int userId, int bookId);
        void ReturnBook(int userId, int bookId);

        Dictionary<int, List<Book>> GetBorrowedBooks();
    }
}
