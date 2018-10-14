using System;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using MovieHunt.ApplicationSettings;
using MovieHunt.MovieDb.Api.Contracts;
using Refit;

namespace MovieHunt.MovieDb.Api
{
    internal class MovieDbApi : IMovieDbApi
    {
        private readonly IMovieDbApi _origin;

        public MovieDbApi(IApiSettings apiSettings)
        {
            _origin = CreateOriginApi(apiSettings);
        }

        public Task<MoviesPageDto> GetUpcomingMovies(int page)
        {
            return _origin.GetUpcomingMovies(page);
        }

        public Task<GenresDto> GetMovieGenres()
        {
            return _origin.GetMovieGenres();
        }

        private static IMovieDbApi CreateOriginApi(IApiSettings apiSettings)
        {
            HttpMessageHandler handler = new NativeMessageHandler();
            handler = new MovieDbAuthHttpClientHandler(apiSettings.ApiKey, handler);

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(apiSettings.BaseUri)
            };
            return RestService.For<IMovieDbApi>(client);
        }
    }
}