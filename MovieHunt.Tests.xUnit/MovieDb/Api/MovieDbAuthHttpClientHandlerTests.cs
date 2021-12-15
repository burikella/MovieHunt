using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MovieHunt.MovieDb.Api.HttpHandlers;
using Xunit;

namespace MovieHunt.Tests.xUnit.MovieDb.Api
{
    public class MovieDbAuthHttpClientHandlerTests
    {
        private readonly string _apiKey;
        private readonly HttpMessageHandler _inner;
        private readonly MovieDbAuthHttpClientHandler _sut;
        private readonly HttpClient _client;

        public MovieDbAuthHttpClientHandlerTests()
        {
            _apiKey = "SomeKey";
            _inner = new DoNothingHandler();
            _sut = new MovieDbAuthHttpClientHandler(_apiKey, _inner);
            _client = new HttpClient(_sut);
        }

        [Fact]
        public void Create_NullApiKey_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new MovieDbAuthHttpClientHandler(null, _inner));
        }

        [Theory]
        [InlineData("http://domain.com/", "http://domain.com/?api_key=SomeKey")]
        [InlineData("http://domain.com/#anchor", "http://domain.com/?api_key=SomeKey#anchor")]
        [InlineData("http://domain.com/path/foo?p=1&x=4", "http://domain.com/path/foo?p=1&x=4&api_key=SomeKey")]
        [InlineData("http://domain.com/?p=1&x=4#abc", "http://domain.com/?p=1&x=4&api_key=SomeKey#abc")]
        [InlineData("http://domain.com/?api_key=#abc", "http://domain.com/?api_key=SomeKey#abc")]
        public async Task SendAsync_NoApiKeyInUrl_ApiKeyAdded(string source, string expected)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, source);
            
            // Act
            await _client.SendAsync(request);

            // Assert
            Assert.Equal(new Uri(expected), request.RequestUri);
        }

        [Theory]
        [InlineData("http://domain.com/?api_key=foo")]
        [InlineData("http://domain.com/?api_key=bar#anchor")]
        [InlineData("http://domain.com/path?api_key=x#anchor")]
        public async Task SendAsync_ApiKeyAlreadyInUrl_ShouldNotChangeUri(string source)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, source);
            
            // Act
            await _client.SendAsync(request);

            // Assert
            Assert.Equal(new Uri(source), request.RequestUri);
        }

        private class DoNothingHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Accepted));
            }
        }
    }
}