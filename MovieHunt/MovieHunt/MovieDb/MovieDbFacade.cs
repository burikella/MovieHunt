using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Akavache;
using MovieHunt.MovieDb.Api.Contracts;
using MovieHunt.UserInterface.ViewModels;

namespace MovieHunt.MovieDb
{
    internal class MovieDbFacade : IMovieDbFacade
    {
        private const string GenresCacheKey = "genres";

        private readonly TimeSpan _genresCacheLifetime = TimeSpan.FromDays(7);
        private readonly IMovieDbApi _api;
        private readonly IBlobCache _cache;

        public MovieDbFacade(IMovieDbApi api, IBlobCache cache)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<MoviesLoadingResult> LoadMovies(int page)
        {
            var result = await _api.GetUpcomingMovies(page);
            var movies = result.Results?.Select(MapMovie) ?? Array.Empty<MovieInfo>();

            return new MoviesLoadingResult(
                result.Page,
                result.TotalPages,
                movies.ToList());
        }

        private Task<IDictionary<int, string>> GetGenreNames()
        {
            return _cache
                .GetOrFetchObject(
                    GenresCacheKey,
                    FetchGenreNames,
                    DateTimeOffset.Now.Add(_genresCacheLifetime))
                .ToTask();
        }

        private async Task<IDictionary<int, string>> FetchGenreNames()
        {
            var result = await _api.GetMovieGenres();
            return result.Genres.ToDictionary(g => g.Id, g => g.Name);
        }

        private static MovieInfo MapMovie(MovieDto movie)
        {
            return new MovieInfo { Title = movie.Title };
        }
    }
}