using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace MovieHunt.Extensions
{
    public static class PropertyChangedExtensions
    {
        public static bool IsFor(this PropertyChangedEventArgs e, params BindableProperty[] properties)
        {
            return properties.Any(x => x.PropertyName.Equals(e.PropertyName));
        }
    }
}