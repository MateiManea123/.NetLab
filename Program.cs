using FluentValidation;
using Microsoft.EntityFrameworkCore;
using BookManagement.Common;
using BookManagement.Features.Books;
using BookManagement.Middleware;
using BookManagement.Persistence;
using BookManagement.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<BookManagementContext>(o => o.UseSqlite("Data Source=bookmanagement.db"));

builder.Services.AddScoped<CreateBookHandler>();
builder.Services.AddScoped<GetAllBooksHandler>();           
builder.Services.AddScoped<ListBooksHandler>();
builder.Services.AddScoped<GetBookByIdHandler>();           
builder.Services.AddScoped<UpdateBookHandler>();            
builder.Services.AddScoped<DeleteBookHandler>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateBookValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBookValidator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookManagementContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseMiddleware<ErrorHandlingMiddleware>();   
app.UseHttpsRedirection();

// Create
app.MapPost("/books", async (CreateBookRequest req, CreateBookHandler h) => await h.Handle(req));

// List (filter + sort + pagination)
app.MapGet("/books", async (
    string? author, BookSortBy sortBy, SortDir dir, int page, int pageSize, ListBooksHandler h
) => await h.Handle(new ListBooksRequest(author, sortBy, dir, page, pageSize)));

// Get by id 
app.MapGet("/books/{id:guid}", async (Guid id, GetBookByIdHandler h) =>
    await h.Handle(new GetBookByIdRequest(id)));

// Update
app.MapPut("/books/{id:guid}", async (Guid id, UpdateBookRequest body, UpdateBookHandler h) =>
    await h.Handle(body with { Id = id }));

// Delete
app.MapDelete("/books/{id:guid}", async (Guid id, DeleteBookHandler h) =>
    await h.Handle(new DeleteBookRequest(id)));

app.Run();
