using System;
using System.Net.Http;
using ModernHttpClient;
using MovieHunt.ApplicationSettings;
using MovieHunt.MovieDb.Api.Contracts;
using Refit;

namespace MovieHunt.MovieDb.Api
{
    internal class MovieDbApiFactory : IMovieDbApiFactory
    {
        private readonly IApiSettings _apiSettings;

        public MovieDbApiFactory(IApiSettings apiSettings)
        {
            _apiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));
        }

        public IMovieDbApi Create()
        {
            HttpMessageHandler handler = new NativeMessageHandler();
            handler = new MovieDbAuthHttpClientHandler(_apiSettings.ApiKey, handler);

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(_apiSettings.BaseUri)
            };
            return RestService.For<IMovieDbApi>(client);
        }
    }
}