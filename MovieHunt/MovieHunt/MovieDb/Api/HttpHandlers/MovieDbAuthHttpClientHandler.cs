using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MovieHunt.MovieDb.Api.HttpHandlers
{
    internal class MovieDbAuthHttpClientHandler : DelegatingHandler
    {
        private const string ApiKeyParameter = "api_key";

        private readonly string _apiKey;

        public MovieDbAuthHttpClientHandler(string apiKey, HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.RequestUri = AddQueryParameterIfNotSet(request.RequestUri, ApiKeyParameter, _apiKey);

            return base.SendAsync(request, cancellationToken);
        }

        private static Uri AddQueryParameterIfNotSet(Uri uri, string name, string value)
        {
            var parameters = HttpUtility.ParseQueryString(uri.Query);
            if (!string.IsNullOrWhiteSpace(parameters.Get(name)))
            {
                return uri;
            }

            parameters[name] = value;

            var uriBuilder = new UriBuilder(uri)
            {
                Query = parameters.ToString()
            };

            return new Uri(uriBuilder.ToString());
        }
    }
}