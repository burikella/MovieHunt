using NSubstitute;
using NUnit.Framework;
using Prism.Navigation;
using Prism.Services;

namespace MovieHunt.Tests.UserInterface.ViewModels
{
    public class ViewModelFixture
    {
        protected INavigationService NavigationService { get; private set; }

        protected IPageDialogService PageDialogService { get; private set; }

        [SetUp]
        public void SetUpFixture()
        {
            NavigationService = Substitute.For<INavigationService>();
            PageDialogService = Substitute.For<IPageDialogService>();
        }
    }
}