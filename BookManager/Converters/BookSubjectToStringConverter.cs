using BookManager.Localization;
using BookManager.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BookManager.Converters;

public class BookSubjectToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BookSubject bookSubject = (BookSubject)value;

        return bookSubject switch
        {
            BookSubject.Fiction => Strings.BookSubject_Fiction,
            BookSubject.Educ => Strings.BookSubject_Educ,
            BookSubject.Health => Strings.BookSubject_Health,
            BookSubject.Politics => Strings.BookSubject_Politics,
            BookSubject.Other => Strings.BookSubject_Other,
            _ => throw new ArgumentException(nameof(bookSubject)),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
