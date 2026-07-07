using BookTracker.Api.Application.BookList;
using BookTracker.Api.Storage;

namespace BookTracker.Api.Application;

public class BookService(IBookRepository bookRepository)
{
    public async Task<IReadOnlyList<BookInfo>> GetAllBooks()
    {
        var books = await bookRepository.GetAllAsync();

        // aquí falta transformar `books` (List<Book>) en una lista de BookInfo
var summary = books.Select(b => new BookInfo
{
    Id = b.Id,
    Title = b.Title,
    Author = b.Author
});
        return [.. summary];
    }
}