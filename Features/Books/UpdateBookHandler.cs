using BookManagement.Common;
using Microsoft.EntityFrameworkCore;
using BookManagement.Persistence;
using BookManagement.Validators;

namespace BookManagement.Features.Books;

public class UpdateBookHandler(BookManagementContext db)
{
    private readonly BookManagementContext _db = db;

    public async Task<IResult> Handle(UpdateBookRequest req)
    {
        var validator = new UpdateBookValidator();
        var vr = await validator.ValidateAsync(req);
        if (!vr.IsValid) return Results.BadRequest(vr.Errors);

        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == req.Id);
        if (book is null) throw new DomainException("Book not found");

        var updated = book with { Title = req.Title, Author = req.Author, Year = req.Year };
        _db.Entry(book).CurrentValues.SetValues(updated);
        await _db.SaveChangesAsync();

        return Results.Ok(updated);
    }
}