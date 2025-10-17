using Microsoft.EntityFrameworkCore;
using BookManagement.Persistence;

namespace BookManagement.Features.Books;

public class ListBooksHandler(BookManagementContext db)
{
    private readonly BookManagementContext _db = db;

    public async Task<IResult> Handle(ListBooksRequest req)
    {
        IQueryable<Book> q = _db.Books.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(req.Author))
            q = q.Where(b => b.Author == req.Author);

        q = (req.SortBy, req.Dir) switch
        {
            (BookSortBy.Title, SortDir.Asc) => q.OrderBy(b => b.Title),
            (BookSortBy.Title, SortDir.Desc) => q.OrderByDescending(b => b.Title),
            (BookSortBy.Year, SortDir.Asc) => q.OrderBy(b => b.Year),
            (BookSortBy.Year, SortDir.Desc) => q.OrderByDescending(b => b.Year),
            _ => q.OrderBy(b => b.Title)
        };

        var page = Math.Max(1, req.Page);
        var pageSize = Math.Clamp(req.PageSize, 1, 200);

        var total = await q.CountAsync();
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return Results.Ok(new { total, page, pageSize, items });
    }
}