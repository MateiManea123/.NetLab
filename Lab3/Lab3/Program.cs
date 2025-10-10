using Lab3;


var books = new List<Book>
{
    new("The Pragmatic Programmer", "Andrew Hunt, David Thomas", 1999),
    new("Clean Code", "Robert C. Martin", 2008),
    new("Domain-Driven Design", "Eric Evans", 2003),
    new("CLR via C#", "Jeffrey Richter", 2012),
    new("C# in Depth", "Jon Skeet", 2019),
};


var librarian = new Librarian
{
    Name = "Ana Ionescu",
    Email = "ana.ionescu@citylibrary.ro",
    LibrarySection = "Computer Science"
};

var borrower1 = new Borrower(
    Id: 1,
    Name: "Mihai Popescu",
    BorrowedBooks: new List<Book> { books[0], books[1] }
);

//Using 'with' expression to create a new borrower with an additional book
var borrower2 = borrower1 with
{
    BorrowedBooks = borrower1.BorrowedBooks
        .Append(books[3]) 
        .ToList()
};


//User Input
Console.WriteLine("Insert book titles:");
while (true)
{
    Console.Write("Title: ");
    var title = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(title))
        break;
    
    books.Add(new Book(title.Trim(), "Unknown", DateTime.Now.Year));
}

Console.WriteLine("\n All books from the list:");
foreach (var b in books)
    Console.WriteLine($"- {b.Title} ({b.YearPublished}) — {b.Author}");

//Pattern Matching
Console.WriteLine("\nPattern Matching:");
TypeIdentifier(books[0]);
TypeIdentifier(borrower2);
TypeIdentifier(42);

//Static lambda filtering
Console.WriteLine("\nBooks published after 2010:");
var recentBooks = books.Where(static b => b.YearPublished > 2010);
foreach (var b in recentBooks)
    Console.WriteLine($"- {b.Title} ({b.YearPublished})");



static void TypeIdentifier(object obj)
{
    switch (obj)
    {
        case Book { Title: var t, YearPublished: var y }:
            Console.WriteLine($"Book -> Title: \"{t}\", Year: {y}");
            break;

        case Borrower { Name: var n, BorrowedBooks: var list }:
            Console.WriteLine($"Borrower -> Name: {n}, Borrowed Books: {list?.Count ?? 0}");
            break;

        default:
            Console.WriteLine("Unknown type");
            break;
    }
}