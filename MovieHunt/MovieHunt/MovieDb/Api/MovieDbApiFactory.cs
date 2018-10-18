using System;
using System.Net.Http;
using ModernHttpClient;
using MovieHunt.ApplicationSettings;
using MovieHunt.MovieDb.Api.Contracts;
using MovieHunt.MovieDb.Api.HttpHandlers;
using Plugin.Connectivity.Abstractions;
using Refit;

namespace MovieHunt.MovieDb.Api
{
    internal class MovieDbApiFactory : IMovieDbApiFactory
    {
        private readonly IApiSettings _apiSettings;
        private readonly IConnectivity _connectivity;

        public MovieDbApiFactory(IApiSettings apiSettings, IConnectivity connectivity)
        {
            _apiSettings = apiSettings ?? throw new ArgumentNullException(nameof(apiSettings));
            _connectivity = connectivity;
        }

        public IMovieDbApi Create()
        {
            HttpMessageHandler handler = new NativeMessageHandler();
            handler = new MovieDbAuthHttpClientHandler(_apiSettings.ApiKey, handler);
            handler = new NetworkProblemHttpClientHandler(_connectivity, handler);

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri(_apiSettings.BaseUri)
            };
            return RestService.For<IMovieDbApi>(client);
        }
    }
}