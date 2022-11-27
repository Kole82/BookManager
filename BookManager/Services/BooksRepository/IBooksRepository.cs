using BookManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManager.Services;

public interface IBooksRepository
{
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task RemoveAsync(Book book);
    Task<IEnumerable<Book>> GetAllAsync();
}
