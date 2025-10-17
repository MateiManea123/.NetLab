using Microsoft.EntityFrameworkCore;
using BookManagement.Persistence;
using BookManagement.Features.Books;

namespace BookManagement.Features.Books;

public class DeleteBookHandler
{
    private readonly BookManagementContext _context;

    public DeleteBookHandler(BookManagementContext context)
    {
        _context = context;
    }

    public async Task<IResult> Handle(DeleteBookRequest request)
    {
        var book = await _context.Books.FindAsync(request.Id);
        if (book is null)
            return Results.NotFound();

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return Results.NoContent();
    }
}