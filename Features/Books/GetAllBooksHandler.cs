using BookManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using BookManagement.Features.Books;
using BookManagement.Features.Books;


namespace BookManagement.Features.Books;

public class GetAllBooksHandler
{
    private readonly BookManagementContext _context;

    public GetAllBooksHandler(BookManagementContext context)
    {
        _context = context;
    }

    public async Task<IResult> Handle(GetAllBookRequest request)
    {
        var books = await _context.Books.ToListAsync();
        return Results.Ok(books);
    }
}