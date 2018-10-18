using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MovieHunt.MovieDb;
using MovieHunt.MovieDb.Models;
using NSubstitute;
using NUnit.Framework;

namespace MovieHunt.Tests.MovieDb
{
    [TestFixture]
    public class MoviesCollectionTests
    {
        [Test]
        public void Create_NullArgument_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesCollection(null));
        }

        [Test]
        public async Task Reset_CallsFirstPageLoading()
        {
            // Arrange
            var facade = Substitute.For<IMovieDbFacade>();
            facade.LoadMovies(Arg.Any<int>())
                .Returns(call => new MoviesLoadingResult(call.Arg<int>(), 0, Array.Empty<MovieInfo>()));
            var sut = new MoviesCollection(facade);

            // Act
            await sut.Reset();

            // Assert
            await facade.Received().LoadMovies(1);
        }

        [Test]
        public async Task LoadingPages_PagesAreIterable()
        {
            // Arrange
            var facade = Substitute.For<IMovieDbFacade>();
            facade.LoadMovies(Arg.Any<int>())
                .Returns(call => new MoviesLoadingResult(
                    call.Arg<int>(),
                    2,
                    Enumerable.Range(0, 5).Select(_ => new MovieInfo()).ToList()));

            var sut = new MoviesCollection(facade);

            // Act
            await sut.Reset();
            await sut.LoadNextPage();

            // Assert
            sut.Should().HaveCount(10);
        }

        [Test]
        public async Task LoadNextPage_CallsRightPageLoading()
        {
            // Arrange
            var facade = Substitute.For<IMovieDbFacade>();
            facade.LoadMovies(Arg.Any<int>())
                .Returns(call => new MoviesLoadingResult(call.Arg<int>(), 5, Array.Empty<MovieInfo>()));
            var sut = new MoviesCollection(facade);

            // Act
            await sut.Reset();
            await sut.LoadNextPage();
            await sut.LoadNextPage();

            // Assert
            await facade.Received().LoadMovies(1);
            await facade.Received().LoadMovies(2);
            await facade.Received().LoadMovies(3);
        }

        [Test]
        public async Task NotAllPagesLoaded_IsCompletelyLoadedShouldBeFalse()
        {
            // Arrange
            var facade = Substitute.For<IMovieDbFacade>();
            facade.LoadMovies(Arg.Any<int>())
                .Returns(call => new MoviesLoadingResult(call.Arg<int>(), 4, Array.Empty<MovieInfo>()));
            var sut = new MoviesCollection(facade);

            // Act
            await sut.Reset();
            await sut.LoadNextPage();
            await sut.LoadNextPage();

            // Assert
            sut.IsCompletelyLoaded.Should().BeFalse();
        }

        [Test]
        public async Task LoadedAllPages_IsCompletelyLoadedShouldBeTrue()
        {
            // Arrange
            var facade = Substitute.For<IMovieDbFacade>();
            facade.LoadMovies(Arg.Any<int>())
                .Returns(call => new MoviesLoadingResult(call.Arg<int>(), 3, Array.Empty<MovieInfo>()));
            var sut = new MoviesCollection(facade);

            // Act
            await sut.Reset();
            await sut.LoadNextPage();
            await sut.LoadNextPage();

            // Assert
            sut.IsCompletelyLoaded.Should().BeTrue();
        }

        [Test]
        public async Task LoadNextPage_AllPagesAlreadyLoaded_ShouldNotCallLoading()
        {
            // Arrange
            var facade = Substitute.For<IMovieDbFacade>();
            facade.LoadMovies(Arg.Any<int>())
                .Returns(call => new MoviesLoadingResult(call.Arg<int>(), 2, Array.Empty<MovieInfo>()));
            var sut = new MoviesCollection(facade);

            // Act
            await sut.Reset();
            await sut.LoadNextPage();
            await sut.LoadNextPage();

            // Assert
            await facade.Received(1).LoadMovies(1);
            await facade.Received(1).LoadMovies(2);
            await facade.DidNotReceive().LoadMovies(3);
        }
    }
}