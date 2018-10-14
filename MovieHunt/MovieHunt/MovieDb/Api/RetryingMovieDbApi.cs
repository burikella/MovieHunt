using System;
using System.Net;
using System.Net.Http;
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

        public RetryingMovieDbApi(IRetrySettings settings, IMovieDbApi origin)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            _retryCount = settings.ApiRetryCount;
            _delay = settings.ApiRetryDelay;
            _origin = origin ?? throw new ArgumentNullException(nameof(origin));
        }

        public Task<MoviesPageDto> GetUpcomingMovies(int page)
        {
            return ExecuteWithRetry(() => _origin.GetUpcomingMovies(page));
        }

        public Task<GenresDto> GetMovieGenres()
        {
            return ExecuteWithRetry(_origin.GetMovieGenres);
        }

        private Task<T> ExecuteWithRetry<T>(Func<Task<T>> func)
        {
            return Policy
                .Handle<WebException>()
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(_retryCount, _ => _delay)
                .ExecuteAsync(func);
        }
    }
}