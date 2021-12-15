using Moq;
using Prism.Navigation;
using Prism.Services;

namespace MovieHunt.Tests.xUnit.UserInterface.ViewModels
{
    public class ViewModelFixture
    {
        public ViewModelFixture()
        {
            NavigationServiceMock = new Mock<INavigationService>();
            PageDialogServiceMock = new Mock<IPageDialogService>();
        }
        
        protected Mock<INavigationService> NavigationServiceMock { get; }

        protected Mock<IPageDialogService> PageDialogServiceMock { get; }
        
    }
}