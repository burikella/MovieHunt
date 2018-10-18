using System;
using System.Threading.Tasks;
using MovieHunt.ApplicationSettings;
using MovieHunt.MovieDb.Api.Contracts;
using Polly;

namespace MovieHunt.MovieDb.Api
{
    internal class RetryingMovieDbApi : IMovieDbApi
    {
        private readonly int _retryCount;
        private readonly TimeSpan _delay;
        private readonly IMovieDbApi _origin;

        public RetryingMovieDbApi(IRetrySettings settings, IMovieDbApiFactory originFactory)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _retryCount = settings.ApiRetryCount;
            _delay = settings.ApiRetryDelay;
            _origin = originFactory?.Create() ?? throw new ArgumentNullException(nameof(originFactory));
        }

        public Task<MoviesPageDto> GetUpcomingMovies(int page)
        {
            return ExecuteWithRetry(() => _origin.GetUpcomingMovies(page));
        }

        public Task<GenresDto> GetMovieGenres()
        {
            return ExecuteWithRetry(_origin.GetMovieGenres);
        }

        public Task<ConfigurationDto> GetConfiguration()
        {
            return ExecuteWithRetry(_origin.GetConfiguration);
        }

        private Task<T> ExecuteWithRetry<T>(Func<Task<T>> func)
        {
            return Policy
                .Handle<NetworkProblemException>()
                .WaitAndRetryAsync(_retryCount, _ => _delay)
                .ExecuteAsync(func);
        }
    }
}