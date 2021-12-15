using MovieHunt.MovieDb.Models;
using MovieHunt.UserInterface.ViewModels;
using MovieHunt.UserInterface.Views;
using Prism.Navigation;
using Xunit;

namespace MovieHunt.Tests.xUnit.UserInterface.ViewModels
{
    public class MovieDetailsPageViewModelTests : ViewModelFixture
    {
        [Fact]
        public void Initialize_ShouldInitMovieProperty()
        {
            // Arrange
            var sut = new MovieDetailsPageViewModel(NavigationServiceMock.Object, PageDialogServiceMock.Object);
            var movieInfo = new MovieInfo();
            var parameters = new NavigationParameters
            {
                { MovieDetailsPage.MovieInfoKey, movieInfo }
            };

            // Act
            sut.Initialize(parameters);

            // Assert
            Assert.Same(movieInfo, sut.Movie);
        }
    }
}