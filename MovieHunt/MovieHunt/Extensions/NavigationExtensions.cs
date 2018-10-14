using System.Threading.Tasks;
using Prism.Navigation;
using Xamarin.Forms;

namespace MovieHunt.Extensions
{
    public static class NavigationExtensions
    {
        private const string PageSeparator = "/";

        public static Task NavigateFromRootAsync(this INavigationService navigationService, params string[] pages)
        {
            return navigationService.NavigateAsync($"{PageSeparator}{string.Join(PageSeparator, pages)}");
        }

        public static Task NavigateFromRootAsync(this INavigationService navigationService,
            NavigationParameters navigationParameters, bool animated, params string[] pages)
        {
            return navigationService.NavigateAsync($"{PageSeparator}{string.Join(PageSeparator, pages)}",
                navigationParameters);
        }

        public static Task NavigateRangeAsync(this INavigationService navigationService,
            NavigationParameters parameters, params string[] pages)
        {
            return navigationService.NavigateAsync(string.Join(PageSeparator, pages),
                parameters, animated: false);
        }

        public static Task NavigateRangeAsync(this INavigationService navigationService,
            NavigationParameters parameters, bool useModalNavigation, params string[] pages)
        {
            return navigationService.NavigateAsync(string.Join(PageSeparator, pages),
                parameters, useModalNavigation, false);
        }

        public static Task NavigateRangeAsync(this INavigationService navigationService, params string[] pages)
        {
            return navigationService.NavigateAsync(string.Join(PageSeparator, pages));
        }

        public static string WrapInNavigationPage(this string page)
        {
            return nameof(NavigationPage) + PageSeparator + page;
        }
    }
}