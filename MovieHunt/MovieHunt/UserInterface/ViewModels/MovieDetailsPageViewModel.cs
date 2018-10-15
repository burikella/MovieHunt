using MovieHunt.MovieDb;
using MovieHunt.MovieDb.Models;
using MovieHunt.UserInterface.Views;
using Prism.Navigation;
using Prism.Services;

namespace MovieHunt.UserInterface.ViewModels
{
    public class MovieDetailsPageViewModel : ViewModel
    {
        private MovieInfo _movie;

        public MovieDetailsPageViewModel(
                INavigationService navigationService,
                IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
        }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            if (parameters.TryGetValue(MovieDetailsPage.MovieInfoKey, out MovieInfo info))
            {
                Movie = info;
            }
        }

        public MovieInfo Movie
        {
            get => _movie;
            private set => SetProperty(ref _movie, value);
        }
    }
}