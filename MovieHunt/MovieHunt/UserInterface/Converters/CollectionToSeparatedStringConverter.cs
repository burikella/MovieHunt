using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace MovieHunt.UserInterface.Converters
{
    public class CollectionToSeparatedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string separator = parameter?.ToString() ?? ", ";
            if (value is IEnumerable enumerable)
            {
                return string.Join(separator, enumerable.Cast<object>());
            }

            throw new NotSupportedException(
                $"The value of type {value.GetType().Name} isn't " +
                $"supported by {nameof(CollectionToSeparatedStringConverter)}.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}