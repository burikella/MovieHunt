using MovieHunt.MovieDb;
using Prism.Navigation;
using Prism.Services;

namespace MovieHunt.UserInterface.ViewModels
{
    public class MovieDetailsPageViewModel : ViewModel
    {
        public MovieDetailsPageViewModel(
                INavigationService navigationService,
                IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
        }
    }
}