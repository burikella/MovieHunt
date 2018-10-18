using System;
using System.Threading.Tasks;
using MovieHunt.MovieDb.Api;
using MovieHunt.Resources;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace MovieHunt.UserInterface.ViewModels
{
    public class ViewModel : BindableBase, INavigationAware, IExceptionHandler
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

        public async Task<bool> TryHandleException(Exception exception)
        {
            string message = exception is NetworkProblemException
                ? Strings.ErrorMessage_NetworkProblems
                : Strings.ErrorMessage_SomethingWentWrong;

            await PagePageDialogService.DisplayAlertAsync(
                Strings.ErrorMessage_Title,
                message,
                Strings.Message_OkButton);

            return true;
        }
    }
}
