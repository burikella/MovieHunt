using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using MovieHunt.MovieDb;
using MovieHunt.MovieDb.Models;
using Xunit;

namespace MovieHunt.Tests.xUnit.MovieDb
{
    public class MoviesCollectionTests
    {
        [Fact]
        public void Create_NullArgument_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesCollection(null));
        }

        [Fact]
        public async Task Reset_CallsFirstPageLoading()
        {
            // Arrange
            var facadeMock = new Mock<IMovieDbFacade>();
            facadeMock
                .Setup(f => f.LoadMovies(It.IsAny<int>()))
                .ReturnsAsync((int page) => new MoviesLoadingResult(page, 0, Array.Empty<MovieInfo>()));

            var sut = new MoviesCollection(facadeMock.Object);

            // Act
            await sut.Reset();

            // Assert
            facadeMock.Verify(f => f.LoadMovies(1), Times.Once);
        }

        [Fact]
        public async Task LoadingPages_PagesAreIterable()
        {
            // Arrange
            var facadeMock = new Mock<IMovieDbFacade>();
            var movies = Enumerable.Range(0, 5).Select(_ => new MovieInfo()).ToList();
            facadeMock
                .Setup(f => f.LoadMovies(It.IsAny<int>()))
                .ReturnsAsync((int page) => new MoviesLoadingResult(page, 2, movies));

            var sut = new MoviesCollection(facadeMock.Object);

            // Act
            await sut.Reset();
            await sut.LoadNextPage();

            // Assert
            Assert.Equal(10, sut.Count);
        }

        [Fact]
        public async Task LoadNextPage_CallsRightPageLoading()
        {
            // Arrange
            var facadeMock = new Mock<IMovieDbFacade>();
            facadeMock
                .Setup(f => f.LoadMovies(It.IsAny<int>()))
                .ReturnsAsync((int page) => new MoviesLoadingResult(page, 5, Array.Empty<MovieInfo>()));
            var sut = new MoviesCollection(facadeMock.Object);

            // Act
            await sut.Reset();
            await sut.LoadNextPage();
            await sut.LoadNextPage();

            // Assert
            facadeMock.Verify(f => f.LoadMovies(1), Times.Once);
            facadeMock.Verify(f => f.LoadMovies(2), Times.Once);
            facadeMock.Verify(f => f.LoadMovies(3), Times.Once);
        }

        [Fact]
        public async Task NotAllPagesLoaded_IsCompletelyLoadedShouldBeFalse()
        {
            // Arrange
            var facadeMock = new Mock<IMovieDbFacade>();
            facadeMock
                .Setup(f => f.LoadMovies(It.IsAny<int>()))
                .ReturnsAsync((int page) => new MoviesLoadingResult(page, 4, Array.Empty<MovieInfo>()));
            var sut = new MoviesCollection(facadeMock.Object);

            // Act
            await sut.Reset();
            await sut.LoadNextPage();
            await sut.LoadNextPage();

            // Assert
            Assert.False(sut.IsCompletelyLoaded);
        }

        [Fact]
        public async Task LoadedAllPages_IsCompletelyLoadedShouldBeTrue()
        {
            // Arrange
            var facadeMock = new Mock<IMovieDbFacade>();
            facadeMock
                .Setup(f => f.LoadMovies(It.IsAny<int>()))
                .ReturnsAsync((int page) => new MoviesLoadingResult(page, 3, Array.Empty<MovieInfo>()));
            var sut = new MoviesCollection(facadeMock.Object);

            // Act
            await sut.Reset();
            await sut.LoadNextPage();
            await sut.LoadNextPage();

            // Assert
            Assert.True(sut.IsCompletelyLoaded);
        }

        [Fact]
        public async Task LoadNextPage_AllPagesAlreadyLoaded_ShouldNotCallLoading()
        {
            // Arrange
            var facadeMock = new Mock<IMovieDbFacade>();
            facadeMock
                .Setup(f => f.LoadMovies(It.IsAny<int>()))
                .ReturnsAsync((int page) => new MoviesLoadingResult(page, 2, Array.Empty<MovieInfo>()));
            var sut = new MoviesCollection(facadeMock.Object);

            // Act
            await sut.Reset();
            await sut.LoadNextPage();
            await sut.LoadNextPage();

            // Assert
            facadeMock.Verify(f => f.LoadMovies(1), Times.Once);
            facadeMock.Verify(f => f.LoadMovies(2), Times.Once);
            facadeMock.Verify(f => f.LoadMovies(3), Times.Never);
        }
    }
}