using BookManager.Models;
using BookManager.ViewModels;
using BookManager.Views;

namespace BookManager.Services;

internal class BookItemViewFactory : IBookItemViewFactory
{
    public BookItemView Create(OpenMode openMode, Book? book = null)
    {
        return new BookItemView(new BookItemContext
        {
            OpenMode = openMode,
            Book = book
        });
    }
}
