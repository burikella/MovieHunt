using MovieHunt.MovieDb;
using Prism.Navigation;

namespace MovieHunt.UserInterface.ViewModels
{
    public class MovieDetailsPageViewModel : ViewModel
    {
        public MovieDetailsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}