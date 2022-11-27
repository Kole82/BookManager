using BookManager.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManager.Data;

internal class BookManagerContext : DbContext
{
    public DbSet<Book> Books { get; set; }

    public BookManagerContext(DbContextOptions<BookManagerContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().ToTable("Book");
    }
}
