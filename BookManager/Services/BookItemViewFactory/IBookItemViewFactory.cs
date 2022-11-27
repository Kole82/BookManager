using BookManager.Models;
using BookManager.ViewModels;
using BookManager.Views;

namespace BookManager.Services;

public interface IBookItemViewFactory
{
    BookItemView Create(OpenMode openMode, Book? book = null);
}
