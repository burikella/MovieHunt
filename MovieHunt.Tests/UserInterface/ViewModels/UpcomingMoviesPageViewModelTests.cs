using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MovieHunt.MovieDb;
using MovieHunt.MovieDb.Models;
using MovieHunt.UserInterface.ViewModels;
using MovieHunt.UserInterface.Views;
using NSubstitute;
using NUnit.Framework;
using Prism.Navigation;

namespace MovieHunt.Tests.UserInterface.ViewModels
{
    public class UpcomingMoviesPageViewModelTests : ViewModelFixture
    {
        private UpcomingMoviesPageViewModel _sut;
        private IMovieDbFacade _movieDbFacade;

        [SetUp]
        public void Setup()
        {
            _movieDbFacade = Substitute.For<IMovieDbFacade>();
            _sut = new UpcomingMoviesPageViewModel(NavigationService, PageDialogService, _movieDbFacade);
        }

        [Test]
        public void Initialize_ShouldLoadFirstPage()
        {
            // Act
            _sut.Initialize(new NavigationParameters());

            // Assert
            _movieDbFacade.Received(1).LoadMovies(1);
        }

        [Test]
        public async Task Initialize_ShouldUpdateMoviesCollection()
        {
            // Arrange
            var movieInfo = new MovieInfo();
            _movieDbFacade
                .LoadMovies(Arg.Any<int>())
                .Returns(call => new MoviesLoadingResult(call.Arg<int>(), 3, new List<MovieInfo>
                {
                    movieInfo
                }));

            // Act
            _sut.Initialize(new NavigationParameters());
            await Task.Delay(50);

            // Assert
            _sut.Movies.Should().HaveCount(1).And.Contain(movieInfo);
        }

        [Test]
        public async Task Refresh_ShouldLoadFirstPage()
        {
            // Act
            _sut.Initialize(new NavigationParameters());
            await Task.Delay(10);

            // Assert
            await _movieDbFacade.Received(1).LoadMovies(1);
        }

        [Test]
        public async Task Refresh_ShouldUpdateMoviesCollection()
        {
            // Arrange
            var movieInfo = new MovieInfo();
            _movieDbFacade
                .LoadMovies(Arg.Any<int>())
                .Returns(call => new MoviesLoadingResult(call.Arg<int>(), 3, new List<MovieInfo>
                {
                    movieInfo
                }));

            // Act
            await _sut.Refresh();

            // Assert
            _sut.Movies.Should().HaveCount(1).And.Contain(movieInfo);
        }

        [Test]
        public async Task Refresh_ShouldSetIsRefreshingWhileProcessing()
        {
            // Arrange
            var tcs = new TaskCompletionSource<MoviesLoadingResult>();
            var result = new MoviesLoadingResult(1, 3, new List<MovieInfo>());
            _movieDbFacade.LoadMovies(Arg.Any<int>()).Returns(call => tcs.Task);

            // Act
            var refreshTask = _sut.Refresh();

            // Assert
            _sut.IsRefreshing.Should().BeTrue();

            // Act
            tcs.SetResult(result);
            await refreshTask;

            // Assert
            _sut.IsRefreshing.Should().BeFalse();
        }

        [Test]
        public async Task LoadMore_NotYetLoadedAll_ShouldLoadNextPage()
        {
            // Arrange
            _movieDbFacade
                .LoadMovies(Arg.Any<int>())
                .Returns(call => new MoviesLoadingResult(call.Arg<int>(), 3, new List<MovieInfo>()));

            _sut.Initialize(new NavigationParameters());
            await Task.Delay(50);

            // Act & Assert
            await _sut.LoadMore();
            await _movieDbFacade.Received(1).LoadMovies(2);

            await _sut.LoadMore();
            await _movieDbFacade.Received(1).LoadMovies(3);
        }

        [Test]
        public async Task LoadMore_AlreadyLoadedAll_ShouldLoadNextPage()
        {
            // Arrange
            _movieDbFacade
                .LoadMovies(Arg.Any<int>())
                .Returns(call => new MoviesLoadingResult(call.Arg<int>(), 1, new List<MovieInfo>()));

            _sut.Initialize(new NavigationParameters());
            await Task.Delay(50);

            // Act & Assert
            await _sut.LoadMore();
            await _movieDbFacade.DidNotReceive().LoadMovies(2);
        }

        [Test]
        public async Task OpenDetails_CallsNavigationToMovieDetailsPage()
        {
            // Act
            await _sut.OpenDetails(new MovieInfo());

            // Assert
            await NavigationService.Received(1).NavigateAsync(nameof(MovieDetailsPage), Arg.Any<INavigationParameters>());
        }

        [Test]
        public async Task OpenDetails_PassesMovieToDetailsPage()
        {
            // Arrange
            var movie = new MovieInfo();

            // Act
            await _sut.OpenDetails(movie);

            // Assert
            await NavigationService.Received(1)
                .NavigateAsync(
                    nameof(MovieDetailsPage),
                    Arg.Is<INavigationParameters>(p => p.GetValue<MovieInfo>(MovieDetailsPage.MovieInfoKey) == movie));
        }
    }
}