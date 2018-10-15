using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MovieHunt.MovieDb.Api;
using NUnit.Framework;

namespace MovieHunt.Tests.MovieDb.Api
{
    [TestFixture]
    public class MovieDbAuthHttpClientHandlerTests
    {
        private string _apiKey;
        private HttpMessageHandler _inner;
        private MovieDbAuthHttpClientHandler _sut;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _apiKey = "SomeKey";
            _inner = new DoNothingHandler();
            _sut = new MovieDbAuthHttpClientHandler(_apiKey, _inner);
            _client = new HttpClient(_sut);
        }

        [Test]
        public void Create_NullApiKey_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new MovieDbAuthHttpClientHandler(null, _inner));
        }

        [TestCase("http://domain.com/", "http://domain.com/?api_key=SomeKey")]
        [TestCase("http://domain.com/#anchor", "http://domain.com/?api_key=SomeKey#anchor")]
        [TestCase("http://domain.com/path/foo?p=1&x=4", "http://domain.com/path/foo?p=1&x=4&api_key=SomeKey")]
        [TestCase("http://domain.com/?p=1&x=4#abc", "http://domain.com/?p=1&x=4&api_key=SomeKey#abc")]
        [TestCase("http://domain.com/?api_key=#abc", "http://domain.com/?api_key=SomeKey#abc")]
        public async Task SendAsync_NoApiKeyInUrl_ApiKeyAdded(string source, string expected)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, source);
            
            // Act
            await _client.SendAsync(request);

            // Assert
            request.RequestUri.Should().BeEquivalentTo(new Uri(expected));
        }

        [TestCase("http://domain.com/?api_key=foo")]
        [TestCase("http://domain.com/?api_key=bar#anchor")]
        [TestCase("http://domain.com/path?api_key=x#anchor")]
        public async Task SendAsync_ApiKeyAlreadyInUrl_ShouldNotChangeUri(string source)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, source);
            
            // Act
            await _client.SendAsync(request);

            // Assert
            request.RequestUri.Should().BeEquivalentTo(new Uri(source));
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