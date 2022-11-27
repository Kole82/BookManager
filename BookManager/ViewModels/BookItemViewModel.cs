using BookManager.Localization;
using BookManager.Models;
using BookManager.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookManager.ViewModels;

internal class BookItemViewModel : ObservableObject
{
    private readonly OpenMode _openMode;
    private readonly IBooksRepository _booksRepository;

    private string _errors;
    private Book _oldValue;

    public string BookItemViewTitle { get; private set; }
    public Book Book { get; set; }

    public string Errors
    {
        get => _errors;
        set => SetProperty(ref _errors, value);
    }

    public bool IsAddButtonVisible => _openMode == OpenMode.Add;
    public bool IsEditButtonVisible => _openMode == OpenMode.Update;
    public bool IsRemoveButtonVisible => _openMode == OpenMode.Remove;
    public bool IsInputEnabled => _openMode != OpenMode.Remove;

    public IAsyncRelayCommand AddBookRelayCommand { get; private set; }
    public IAsyncRelayCommand UpdateBookRelayCommand { get; private set; }
    public IAsyncRelayCommand RemoveBookRelayCommand { get; private set; }

    public event EventHandler Completed;

    public BookItemViewModel(IBooksRepository booksRepository, BookItemContext bookItemContext)
    {
        _booksRepository = booksRepository;
        _openMode = bookItemContext.OpenMode;

        AddBookRelayCommand = new AsyncRelayCommand(AddBook);
        UpdateBookRelayCommand = new AsyncRelayCommand(UpdateBook);
        RemoveBookRelayCommand = new AsyncRelayCommand(RemoveBook);

        Init(bookItemContext);
    }

    public async Task AddBook()
    {
        if (IfBookHasErrors())
            return;

        await _booksRepository.AddAsync(Book);

        Completed?.Invoke(null, null);
    }

    public async Task UpdateBook()
    {
        if (IfBookHasErrors())
            return;

        await _booksRepository.UpdateAsync(Book);

        Completed?.Invoke(null, null);
    }

    public async Task RemoveBook()
    {
        await _booksRepository.RemoveAsync(Book);

        Completed?.Invoke(null, null);
    }

    public void RestoreValues()
    {
        Book.Name = _oldValue.Name;
        Book.Author = _oldValue.Name;
        Book.BookSubject = _oldValue.BookSubject;
        Book.PublicationYear = _oldValue.PublicationYear;
    }

    private void Init(BookItemContext bookItemContext)
    {
        switch (bookItemContext.OpenMode)
        {
            case OpenMode.Add:
                Book = new Book();
                BookItemViewTitle = Strings.AddBook;
                break;
            case OpenMode.Update:
                Book = bookItemContext.Book;
                BookItemViewTitle = Strings.UpdateBook;
                break;
            case OpenMode.Remove:
                Book = bookItemContext.Book;
                BookItemViewTitle = Strings.ReallyWantToRemove;
                break;
            default:
                throw new ArgumentException(bookItemContext.OpenMode.ToString());
        }

        _oldValue = new Book
        {
            Id = Book.Id,
            Name = Book.Name,
            Author = Book.Author,
            BookSubject = Book.BookSubject,
            PublicationYear = Book.PublicationYear
        };

        Book.ErrorsChanged += (s, e) => Errors = GetValidationErrors();
    }

    private string GetValidationErrors()
    {
        string errors = string.Empty;

        foreach (ValidationResult validationResult in Book.GetErrors())
        {
            string error = validationResult.ErrorMessage;
            string propertyName = validationResult.MemberNames.FirstOrDefault();

            errors += $"{propertyName}: {error}{Environment.NewLine}";
        }

        return errors;
    }

    private bool IfBookHasErrors()
    {
        Book.Validate();

        return Book.HasErrors;
    }
}
