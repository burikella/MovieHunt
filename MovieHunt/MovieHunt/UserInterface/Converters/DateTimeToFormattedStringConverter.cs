using System;
using System.Globalization;
using Xamarin.Forms;

namespace MovieHunt.UserInterface.Converters
{
    public class DateTimeToFormattedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = (DateTime?) value;
            return dateTime.HasValue ? dateTime.Value.ToString(parameter?.ToString() ?? "D") : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}