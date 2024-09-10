
using System;

public record Book(string ID, string Title, string Author, string Genre, bool IsAvailable);

public record Member(string ID, string Name, string Email);
public record BorrowRecord(string BookId, string MemberId, string Title, string MemberName, DateTime Date);

class LibraryManager
{
    List<Book> bookList = new List<Book>();
    List<Member> memberList = new List<Member>();
    List<BorrowRecord> browList = new List<BorrowRecord>();

    public void AddBook(Book book)
    {
        if (!bookList.Contains(book))
        {
            bookList.Add(book);
            Console.WriteLine($"Book {book.Title} has been successfully added to the library.");
        }
        else
        {
            Console.WriteLine($"Sorry, {book.Title} book is already Added!");
        }
    }
    public void RemoveBook(Book book)
    {
        if (bookList.Contains(book))
        {
            bookList.Remove(book);
            Console.WriteLine($"book{book.Title} has been successfully removed from library");
        }
        else
        {
            Console.WriteLine($"Sorry, {book.Title} book is not in the book list to remove it");
        }
    }
    public void DisplayBooks()
    {
        if (bookList.Any())
        {
            Console.WriteLine($"Library Books:");
            foreach (var item in bookList)
            {
                Console.WriteLine($"{item}");
            }
        }
        else
        {
            Console.WriteLine($"There is ni book in the book list");

        }
    }
    public void SearchByTitle(String title)
    {
        var book = bookList.FirstOrDefault(book => book.Title == title);
        if (book != null)
            Console.WriteLine($"{book}");
        else
        {
            Console.WriteLine($"This book was not found ");
        }
    }

    public void SearchByGenre(String genre)
    {
        var book = bookList.FirstOrDefault(book => book.Genre == genre);
        if (book != null)
            Console.WriteLine($"{book}");
        else
        {
            Console.WriteLine($"This book was not found ");
        }
    }
    public void SearchByAuthor(String author)
    {
        var book = bookList.FirstOrDefault(book => book.Author == author);
        if (book != null)
            Console.WriteLine($"{book}");
        else
        {
            Console.WriteLine($"This book was not found ");
        }
    }
    public void AddMember(Member member)
    {
        memberList.Add(member);
        Console.WriteLine($"Member '{member.Name}' has been successfully added to the library.");
    }
    public void RemoveMember(Member member)
    {
        memberList.Remove(member);
        Console.WriteLine($"Member '{member.Name}' has been successfully removed to the library.");
    }
    public void DisplayMmbers()
    {
        Console.WriteLine($"Library Members:");

        foreach (var item in memberList)
        {
            Console.WriteLine($"{item}");
        }
    }
    public void SearchMemberByName(string name)
    {
        var member = memberList.FirstOrDefault(member => member.Name == name);
        if (member != null)
        {
            Console.WriteLine($"{member}");
        }
        else
        {
            Console.WriteLine($"Member was not found ");
        }
    }
    public void BorrowBook(string bookId, string memberId)
    {
        var memberInList = memberList.FirstOrDefault(member => member.ID == memberId);

        var bookInList = bookList.FirstOrDefault(book => book.ID == bookId);
        if (bookInList != null && bookInList.IsAvailable)
        {
            Book updatedBook = bookInList with { IsAvailable = false };
            bookList.Remove(bookInList);
            bookList.Add(updatedBook);
            BorrowRecord borrowBook = new BorrowRecord(updatedBook.ID, memberInList.ID, updatedBook.Title, memberInList.Name, DateTime.Now);
            browList.Add(borrowBook);
            Console.WriteLine($"The {bookInList.Title} book is borrowed by {borrowBook.MemberName}");
        }
        else
        {
            Console.WriteLine($"{bookId} book is not available");
        }
    }

    public void ReturnBook(string bookId, string memberId)
    {
        var memberInList = memberList.FirstOrDefault(member => member.ID == memberId);

        var bookInList = bookList.FirstOrDefault(book => book.ID == bookId);
        var borrowInList = browList.FirstOrDefault(borrowRecord => borrowRecord.BookId == bookId && borrowRecord.MemberId == memberId);
        if (borrowInList == null)
        {
            Console.WriteLine("This book was not borrowed by this member.");
            return;
        }
        if (!bookInList.IsAvailable)
        {
            var updatedBook = bookInList with { IsAvailable = true };
            bookList.Remove(bookInList);
            bookList.Add(updatedBook);
            browList.Remove(browList.FirstOrDefault(borrowBook => borrowBook.Title == bookInList.Title));
            Console.WriteLine($"The book is returned by {memberInList.Name}");
        }
        else
        {
            Console.WriteLine($"The book with ID : {bookId} is already returned");
        }
    }
    public void DisplayBorrow()
    {
        Console.WriteLine($"Borrow books:");


        if (browList.Any())
        {
            foreach (var item in browList)
            {
                Console.WriteLine($"{item.Title} borrowed by {item.MemberName} on {item.Date}");

            }
        }
        else
        {
            Console.WriteLine($"There is no borrow books!");
        }

    }
    class App
    {
        public static void Main(string[] args)
        {
            LibraryManager list = new LibraryManager();
            Book b1 = new Book("5555", "The Catcher in the Rye", "Salinger", "Fiction", true);
            Book b2 = new Book("7777", "The Great Gatsby", "Scott Fitzgerald", "Classic", true);
            Book b3 = new Book("2222", "The Hobbit", "Tolkien", "Fantasy", true);
            Book b4 = new Book("6666", "Pride and Prejudice", "Jane Austen", "Romance", true);
            Book b5 = new Book("1111", "Game of Thrones", "George Martin", "Fantasy", true);
            Book b6 = new Book("3333", "1984", "George Orwell", "Dystopian", true);
            Book b7 = new Book("4444", "To Kill a Mockingbird", "Harper Lee", "Fiction", true);
            Member m1 = new Member("123", "shahad", "shahad@gami.com");
            Member m2 = new Member("456", "nora", "nora@gami.com");

            list.AddMember(m1);
            list.AddMember(m2);

            list.AddBook(b1);
            list.AddBook(b2);
            list.AddBook(b3);
            list.AddBook(b4);
            list.AddBook(b5);
            list.AddBook(b6);
            list.AddBook(b7);

            // Try to add a book that is already in the Library
            list.AddBook(b1);
            list.DisplayBooks();
            // Remove a book  from the Library
            list.RemoveBook(b7);
            // Check if book number 7 was removed from the Library
            list.DisplayBooks();
            // ry to remove a book that does not exist in the Library
            list.RemoveBook(b7);

            list.BorrowBook(b2.ID, m1.ID);
            list.DisplayBorrow();

            // Try to return the book by another member who has not borrowed the book
            list.ReturnBook(b2.ID, m2.ID);
            // Search for a book by the title
            list.SearchByTitle("Game of Thrones");
            // Try to pass a book title that is not in the Library
            list.SearchByTitle("love");
            // Search for a member by name.
            list.SearchMemberByName("shahad");
            // Try to pass a user name that is not in the Library.
            list.SearchMemberByName("sara");
        }
    }
}