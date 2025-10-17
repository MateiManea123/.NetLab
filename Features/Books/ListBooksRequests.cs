namespace BookManagement.Features.Books;

public enum BookSortBy { Title, Year }
public enum SortDir { Asc, Desc }

public record ListBooksRequest(
    string? Author,
    BookSortBy SortBy = BookSortBy.Title,
    SortDir Dir = SortDir.Asc,
    int Page = 1,
    int PageSize = 10
);
