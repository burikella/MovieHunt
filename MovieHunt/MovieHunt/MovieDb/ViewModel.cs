using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace MovieHunt.MovieDb
{
    public class ViewModel : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; }
        protected IPageDialogService PagePageDialogService { get; }

        public ViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PagePageDialogService = pageDialogService;
        }

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
