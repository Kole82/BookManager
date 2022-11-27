using BookManager.Data;
using BookManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManager.Services;

internal class BooksRepository : IBooksRepository
{
    private readonly BookManagerContext _bookManagerContext;

    public BooksRepository(BookManagerContext bookManagerContext)
    {
        _bookManagerContext = bookManagerContext;
    }

    public async Task AddAsync(Book book)
    {
        await _bookManagerContext.Books.AddAsync(book);
        await _bookManagerContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _bookManagerContext.Update(book);

        await _bookManagerContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(Book book)
    {
        _bookManagerContext.Books.Remove(book);

        await _bookManagerContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _bookManagerContext.Books.ToListAsync();
    }
}
