using FluentAssertions;
using MovieHunt.MovieDb.Models;
using MovieHunt.UserInterface.ViewModels;
using MovieHunt.UserInterface.Views;
using NUnit.Framework;
using Prism.Navigation;

namespace MovieHunt.Tests.UserInterface.ViewModels
{
    public class MovieDetailsPageViewModelTests : ViewModelFixture
    {
        [Test]
        public void Initialize_ShouldInitMovieProperty()
        {
            // Arrange
            var sut = new MovieDetailsPageViewModel(NavigationService, PageDialogService);
            var movieInfo = new MovieInfo();
            var parameters = new NavigationParameters
            {
                { MovieDetailsPage.MovieInfoKey, movieInfo }
            };

            // Act
            sut.Initialize(parameters);

            // Assert
            sut.Movie.Should().Be(movieInfo);
        }
    }
}