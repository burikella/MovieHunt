using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using MovieHunt.MovieDb;

namespace MovieHunt.UserInterface.ViewModels
{
    internal class MoviesCollection : ObservableCollection<MovieInfo>, INotifyCollectionChanged
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IMovieDbFacade _movieDb;

        private int _lastLoadedPage;
        private int _totalPages;

        public MoviesCollection(IMovieDbFacade movieDb)
        {
            _movieDb = movieDb;
        }

        public Task Reset() => RunSunchronized(ResetInternal);

        public Task LoadNextPage() => RunSunchronized(LoadNextPageInternal);

        public bool IsCompletelyLoaded => _totalPages == _lastLoadedPage;

        private async Task RunSunchronized(Func<Task> action)
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                action?.Invoke();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private Task ResetInternal()
        {
            Clear();
            return LoadPage(1);
        }

        private Task LoadNextPageInternal()
        {
            return LoadPage(_lastLoadedPage + 1);
        }

        private async Task LoadPage(int page)
        {
            if (_lastLoadedPage > 0 && page > _totalPages)
            {
                return;
            }

            var loadingResult = await _movieDb.LoadMovies(page).ConfigureAwait(false);
            foreach (var movie in loadingResult.Movies)
            {
                Add(movie);
            }

            _totalPages = loadingResult.TotalPages;
            _lastLoadedPage = page;
        }
    }
}