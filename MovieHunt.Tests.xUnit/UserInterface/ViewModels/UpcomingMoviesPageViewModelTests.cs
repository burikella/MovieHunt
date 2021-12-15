using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using MovieHunt.MovieDb;
using MovieHunt.MovieDb.Models;
using MovieHunt.UserInterface.ViewModels;
using MovieHunt.UserInterface.Views;
using Prism.Navigation;
using Xunit;

namespace MovieHunt.Tests.xUnit.UserInterface.ViewModels
{
    public class UpcomingMoviesPageViewModelTests : ViewModelFixture
    {
        private UpcomingMoviesPageViewModel _sut;
        private Mock<IMovieDbFacade> _movieDbFacadeMock;

        public UpcomingMoviesPageViewModelTests()
        {
            _movieDbFacadeMock = new Mock<IMovieDbFacade>();
            _sut = new UpcomingMoviesPageViewModel(
                NavigationServiceMock.Object,
                PageDialogServiceMock.Object,
                _movieDbFacadeMock.Object);
        }

        [Fact]
        public void Initialize_ShouldLoadFirstPage()
        {
            // Act
            _sut.Initialize(new NavigationParameters());

            // Assert
            _movieDbFacadeMock.Verify(f => f.LoadMovies(1));
        }

        [Fact]
        public async Task Initialize_ShouldUpdateMoviesCollection()
        {
            // Arrange
            var movieInfo = new MovieInfo();
            _movieDbFacadeMock
                .Setup(f => f.LoadMovies(It.IsAny<int>()))
                .ReturnsAsync((int page) =>
                    new MoviesLoadingResult(page, 3, new List<MovieInfo>
                    {
                        movieInfo
                    }));

            // Act
            _sut.Initialize(new NavigationParameters());
            await Task.Delay(50);

            // Assert
            Assert.Collection(_sut.Movies, movie => Assert.Equal(movieInfo, movie));
        }

        [Fact]
        public void Refresh_ShouldLoadFirstPage()
        {
            // Act
            _sut.Initialize(new NavigationParameters());

            // Assert
            _movieDbFacadeMock.Verify(f => f.LoadMovies(1));
        }

        [Fact]
        public async Task Refresh_ShouldUpdateMoviesCollection()
        {
            // Arrange
            var movieInfo = new MovieInfo();

            _movieDbFacadeMock
                .Setup(f => f.LoadMovies(It.IsAny<int>()))
                .ReturnsAsync((int page) =>
                    new MoviesLoadingResult(page, 3, new List<MovieInfo>
                    {
                        movieInfo
                    }));

            // Act
            await _sut.Refresh();

            // Assert
            Assert.Collection(_sut.Movies, movie => Assert.Equal(movieInfo, movie));
        }

        [Fact]
        public async Task Refresh_ShouldSetIsRefreshingWhileProcessing()
        {
            // Arrange
            var tcs = new TaskCompletionSource<MoviesLoadingResult>();
            var result = new MoviesLoadingResult(1, 3, new List<MovieInfo>());
            _movieDbFacadeMock
                .Setup(f => f.LoadMovies(It.IsAny<int>()))
                .Returns(tcs.Task);

            // Act
            var refreshTask = _sut.Refresh();

            // Assert
            Assert.True(_sut.IsRefreshing);

            // Act
            tcs.SetResult(result);
            await refreshTask;

            // Assert
            Assert.False(_sut.IsRefreshing);
        }

        [Fact]
        public async Task LoadMore_NotYetLoadedAll_ShouldLoadNextPage()
        {
            // Arrange
            _movieDbFacadeMock
                .Setup(f => f.LoadMovies(It.IsAny<int>()))
                .ReturnsAsync((int page) =>
                    new MoviesLoadingResult(page, 3, new List<MovieInfo>()));

            _sut.Initialize(new NavigationParameters());
            await Task.Delay(50);

            // Act & Assert
            await _sut.LoadMore();
            _movieDbFacadeMock.Verify(f => f.LoadMovies(2), Times.Once);

            await _sut.LoadMore();
            _movieDbFacadeMock.Verify(f => f.LoadMovies(3), Times.Once);
        }

        [Fact]
        public async Task LoadMore_AlreadyLoadedAll_ShouldLoadNextPage()
        {
            // Arrange
            _movieDbFacadeMock
                .Setup(f => f.LoadMovies(It.IsAny<int>()))
                .ReturnsAsync((int page) => new MoviesLoadingResult(page, 1, new List<MovieInfo>()));

            _sut.Initialize(new NavigationParameters());
            await Task.Delay(50);

            // Act & Assert
            await _sut.LoadMore();
            _movieDbFacadeMock.Verify(f => f.LoadMovies(2), Times.Never);
        }

        [Fact]
        public async Task OpenDetails_CallsNavigationToMovieDetailsPage()
        {
            // Act
            await _sut.OpenDetails(new MovieInfo());

            // Assert
            NavigationServiceMock.Verify(s => s.NavigateAsync(nameof(MovieDetailsPage), It.IsAny<INavigationParameters>()));
        }

        [Fact]
        public async Task OpenDetails_PassesMovieToDetailsPage()
        {
            // Arrange
            var movie = new MovieInfo();

            // Act
            await _sut.OpenDetails(movie);

            // Assert
            NavigationServiceMock.Verify(
                s => s.NavigateAsync(
                    nameof(MovieDetailsPage),
                    It.Is<INavigationParameters>(p => p.GetValue<MovieInfo>(MovieDetailsPage.MovieInfoKey) == movie)));
        }
    }
}