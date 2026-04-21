using BookLab5.Models;
namespace BookLab5.Services
{
    public class LibraryService : ILibraryService
    {
        public bool useFiles = true;
        public List<Book> books = new();
        public List<User> users = new();
        public Dictionary<int, List<Book>> borrowedBooks = new();

        public readonly string booksFile = "Data/Books.csv";
        public readonly string usersFile = "Data/Users.csv";

        public LibraryService(bool useFiles = true)
        {
            this.useFiles = useFiles;

            if (useFiles)
            {
                ReadBooks();
                LoadUsers();
            }
        }

        ///READ
        ///

        public void ReadBooks()
        {
            if (!File.Exists(booksFile)) return;

            foreach (var line in File.ReadLines(booksFile))
            {
                var fields = line.Split(',');

                if (fields.Length >= 4)
                {
                    books.Add(new Book
                    {
                        Id = int.Parse(fields[0].Trim()),
                        Title = fields[1].Trim(),
                        Author = fields[2].Trim(),
                        ISBN = fields[3].Trim(),
                    });
                }
            }
        }

        public void ReadUsers()
        {
            if (!File.Exists(usersFile)) return;

            foreach (var line in File.ReadLines(usersFile))
            {
                var fields = line.Split(",");

                if (fields.Length >= 3)
                {
                    users.Add(new User
                    {
                        Id = int.Parse(fields[0].Trim()),
                        Name = fields[1].Trim(),
                        Email = fields[2].Trim(),
                    });
                }
            }
        }

        ///WRITE
        ///

        public void SaveBooks()
        {
            if (!useFiles) return;
            var lines = books.Select(b => $"{b.Id},{b.Title},{b.Author},{b.ISBN}");
            File.WriteAllLines(booksFile, lines);
        }

        public void SaveUsers()
        {
            if (!useFiles) return;
            var lines = users.Select(u => $"{u.Id},{u.Name},{u.Email}");
            File.WriteAllLines(usersFile, lines);
        }

        ///GET
        ///

        public List<Book> GetBooks() => books;
        public List<User> GetUsers() => users;
        public Dictionary<int, List<Book>> GetBorrowedBooks() => borrowedBooks;

        ///BOOK CRUD
        ///

        public void AddBook(Book book)
        {
            book.Id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
            SaveBooks();
        }

        public void EditBook(Book book)
        {
            var existing = books.FirstOrDefault(b => b.Id == book.Id);
            if (existing != null)
            {
                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.ISBN = book.ISBN;
                SaveBooks();
            }
        }

        public void UpdateBook(Book updatedBook)
        {
            var book = books.FirstOrDefault(b => b.Id == updatedBook.Id);

            if (book != null)
            {
                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.ISBN = updatedBook.ISBN;

                SaveBooks();
            }
        }
        public void DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
                SaveBooks();
            }
        }

        //User CRUD

        public void LoadUsers()
        {
            if (!File.Exists("Data/Users.csv"))
                return;

            users = File.ReadAllLines("Data/Users.csv")
                .Select(line =>
                {
                    var parts = line.Split(',');
                    return new User
                    {
                        Id = int.Parse(parts[0]),
                        Name = parts[1],
                        Email = parts[2]
                    };
                })
                .ToList();

        }

        public void AddUser(User user)
        {
            user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            SaveUsers();
        }

        public void EditUser(User user)
        {
            var existing = users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                existing .Name = user.Name;
                existing.Email = user.Email;
                SaveUsers();
            }
        }

        public void DeleteUser (int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                users.Remove(user);
                SaveUsers();
            }
        }

        ///BORROW & RETURN
        ///

        public void BorrowBook(int userId, int bookId)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            var book = books.FirstOrDefault(b => b.Id == bookId);

            if (user == null || book == null) return;

            if (!borrowedBooks.ContainsKey(userId))
            {
                borrowedBooks[userId] = new List<Book>();
            }

            borrowedBooks[userId].Add(book);
            books.Remove(book);

            SaveBooks();
        }

        public void ReturnBook(int userId, int bookId)
        {
            if (!borrowedBooks.ContainsKey(userId)) return;

            var book = borrowedBooks[userId].FirstOrDefault(b => b.Id == bookId);

            if (book != null)
            {
                borrowedBooks[userId].Remove(book);
                books.Add(book);

                SaveBooks();
            }
        }
    }
}
