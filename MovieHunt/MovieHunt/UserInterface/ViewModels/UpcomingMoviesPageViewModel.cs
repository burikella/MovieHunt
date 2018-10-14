using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MovieHunt.MovieDb;
using MovieHunt.Utility;
using Prism.Navigation;

namespace MovieHunt.UserInterface.ViewModels
{
    internal class UpcomingMoviesPageViewModel : ViewModel
    {
        private readonly MoviesCollection _movies;
        private readonly OperationsCounter _operationsCounter;

        private bool _isRefreshing;

        public UpcomingMoviesPageViewModel(
                INavigationService navigationService,
                IMovieDbFacade movieDbFacade)
            : base(navigationService)
        {
            _movies = new MoviesCollection(movieDbFacade);
            _operationsCounter = new OperationsCounter(state => IsRefreshing = state);
        }

        public ICollection<MovieInfo> Movies => _movies;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public override async void OnNavigatingTo(NavigationParameters parameters)
        {
            await Refresh();
        }

        public async Task Refresh()
        {
            using (_operationsCounter.Run())
            {
                await _movies.Reset();
            }
        }

        public Task LoadMore() => _movies.LoadNextPage();
    }
}
