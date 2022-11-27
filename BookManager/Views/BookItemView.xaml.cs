using BookManager.Services;
using BookManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace BookManager.Views;

public partial class BookItemView : Window
{
    public BookItemView(BookItemContext bookItemContext)
    {
        var booksRepository = App.AppHost!.Services.GetRequiredService<IBooksRepository>();
        var bookItemViewModel = new BookItemViewModel(booksRepository, bookItemContext);

        bookItemViewModel.Completed += (s, e) => Close();
        KeyDown += (s, e) =>
        {
            if (e.Key == Key.Escape)
            {
                bookItemViewModel.RestoreValues();
                Close();
            }
        };

        DataContext = bookItemViewModel;
        Owner = App.AppHost!.Services.GetRequiredService<BookCollectionView>();

        InitializeComponent();
    }
}
