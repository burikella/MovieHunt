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

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
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