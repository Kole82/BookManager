using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookManager.Models;

public class Book : ObservableValidator
{
    private string? _name;
    private string? _author;
    private BookSubject _bookSubject;
    private int? _publicationYear;

    public int Id { get; set; }

    [Required]
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value, true);
    }

    [Required]
    [MinLength(5, ErrorMessage = "Author name is too short")]
    [MaxLength(50, ErrorMessage = "Author name is too long")]
    public string? Author
    {
        get => _author;
        set => SetProperty(ref _author, value, true);
    }

    public BookSubject BookSubject
    {
        get => _bookSubject;
        set => SetProperty(ref _bookSubject, value);
    }

    [Required]
    [Range(1955, 2022, ErrorMessage = "Publication year is out of range")]
    public int? PublicationYear
    {
        get => _publicationYear;
        set => SetProperty(ref _publicationYear, value, true);
    }

    public void Validate()
    {
        ValidateProperty(Name, nameof(Name));
        ValidateProperty(Author, nameof(Author));
        ValidateProperty(PublicationYear, nameof(PublicationYear));
    }
}
