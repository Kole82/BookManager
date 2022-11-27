using BookManager.Services;
using BookManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BookManager.Views;

public partial class BookCollectionView : Window
{
    GridViewColumnHeader _lastHeaderClicked = null;
    ListSortDirection _lastDirection = ListSortDirection.Ascending;

    public BookCollectionView()
    {
        var booksRepository = App.AppHost!.Services.GetRequiredService<IBooksRepository>();

        DataContext = new BookCollectionViewModel(booksRepository);

        InitializeComponent();
    }

    private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
    {
        ListSortDirection direction;

        if (e.OriginalSource is not GridViewColumnHeader headerClicked ||
            headerClicked.Role == GridViewColumnHeaderRole.Padding)
            return;

        if (headerClicked != _lastHeaderClicked)
            direction = ListSortDirection.Ascending;
        else
        {
            direction = (_lastDirection == ListSortDirection.Ascending)
                ? ListSortDirection.Descending : ListSortDirection.Ascending;
        }

        Binding columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
        string sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

        Sort(sortBy, direction);

        string resource = (direction == ListSortDirection.Ascending)
            ? "HeaderTemplateArrowUp" : "HeaderTemplateArrowDown";

        headerClicked.Column.HeaderTemplate = Resources[resource] as DataTemplate;

        if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
            _lastHeaderClicked.Column.HeaderTemplate = null;

        _lastHeaderClicked = headerClicked;
        _lastDirection = direction;
    }

    private void Sort(string sortBy, ListSortDirection direction)
    {
        ICollectionView dataView =
          CollectionViewSource.GetDefaultView(BooksView.ItemsSource);

        dataView.SortDescriptions.Clear();
        var sortDescription = new SortDescription(sortBy, direction);
        dataView.SortDescriptions.Add(sortDescription);
        dataView.Refresh();
    }
}