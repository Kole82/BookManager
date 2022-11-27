using BookManager.Models;
using BookManager.Services;
using BookManager.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BookManager.ViewModels;

public class BookCollectionViewModel : ObservableObject
{
    private readonly IBooksRepository _booksRepository;

    private Book _selectedBook;
    private bool _isBusyIndicatorVisible;

    public IRelayCommand AddBookRelayCommand { get; private set; }
    public IRelayCommand UpdateBookRelayCommand { get; private set; }
    public IRelayCommand RemoveBookRelayCommand { get; private set; }
    public IAsyncRelayCommand RefreshBooksRelayCommand { get; private set; }

    public Book? SelectedBook
    {
        get => _selectedBook;
        set
        {
            _selectedBook = value;
            OnPropertyChanged(nameof(IsUpdateAndRemoveEnabled));
        }
    }

    public bool IsBusyIndicatorVisible
    {
        get => _isBusyIndicatorVisible;
        set
        {
            SetProperty(ref _isBusyIndicatorVisible, value);
            OnPropertyChanged(nameof(IsUIEnabled));
        }
    }

    public bool IsUpdateAndRemoveEnabled => SelectedBook != null;
    public bool IsUIEnabled => !IsBusyIndicatorVisible;

    public ObservableCollection<Book> Books { get; private set; } = new();

    public BookCollectionViewModel(IBooksRepository booksRepository)
    {
        _booksRepository = booksRepository;

        RefreshBooksRelayCommand = new AsyncRelayCommand(RefreshBooks);
       
        AddBookRelayCommand = new RelayCommand(AddBook);
        UpdateBookRelayCommand = new RelayCommand(UpdateBook);
        RemoveBookRelayCommand = new RelayCommand(RemoveBook);

        RefreshBooksRelayCommand.Execute(null);
    }

    public void AddBook()
    {
        BookItemView bookItemView = App.AppHost!.Services
            .GetRequiredService<IBookItemViewFactory>()
            .Create(OpenMode.Add);

        bookItemView.ShowDialog();

        RefreshBooksRelayCommand.Execute(null);
    }

    public void UpdateBook()
    {
        if (SelectedBook == null)
            return;

        BookItemView bookItemView = App.AppHost!.Services
            .GetRequiredService<IBookItemViewFactory>()
            .Create(OpenMode.Update, SelectedBook);

        bookItemView.ShowDialog();

        RefreshBooksRelayCommand.Execute(null);
    }

    public void RemoveBook()
    {
        if (SelectedBook == null)
            return;

        BookItemView bookItemView = App.AppHost!.Services
            .GetRequiredService<IBookItemViewFactory>()
            .Create(OpenMode.Remove, SelectedBook);

        bookItemView.ShowDialog();

        RefreshBooksRelayCommand.Execute(null);
    }

    public async Task RefreshBooks()
    {
        IsBusyIndicatorVisible = true;

        var items = await _booksRepository.GetAllAsync();

        Books.Clear();

        foreach (Book book in items)
            Books.Add(book);

        IsBusyIndicatorVisible = false;
    }
}

public enum OpenMode
{
    Add,
    Remove,
    Update
}

public class BookItemContext
{
    public Book? Book { get; set; }
    public OpenMode OpenMode { get; set; }
}
