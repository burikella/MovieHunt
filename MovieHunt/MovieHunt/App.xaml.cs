using System;
using Akavache;
using DryIoc;
using MovieHunt.ApplicationSettings;
using MovieHunt.MovieDb;
using MovieHunt.MovieDb.Api;
using MovieHunt.MovieDb.Api.Contracts;
using MovieHunt.UserInterface.ViewModels;
using MovieHunt.UserInterface.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.DryIoc;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MovieHunt
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            try
            {
                await NavigationService.NavigateAsync("NavigationPage/UpcomingMoviesPage");
            }
            catch (Exception e)
            {
                throw;
            }
        }
        
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var settings = new AppSettings
            {
                ApplicationName = "MovieHunt",

                ApiKey = "<KEY SHOULD BE HERE>",
                BaseUri = "https://api.themoviedb.org/3",

                ApiRetryCount = 3,
                ApiRetryDelay = TimeSpan.FromMilliseconds(250)
            };

            BlobCache.ApplicationName = settings.ApplicationName;

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<UpcomingMoviesPage>();
            containerRegistry.RegisterForNavigation<MovieDetailsPage>();

            var container = containerRegistry.GetContainer();

            container.UseInstance<IApiSettings>(settings);
            container.UseInstance<IRetrySettings>(settings);

            container.Register<UpcomingMoviesPageViewModel>();
            container.Register<IMovieDbApi, MovieDbApi>();
            container.RegisterInstance<IBlobCache>(BlobCache.UserAccount);
            container.Register<IMovieDbApi, RetryingMovieDbApi>(setup: Setup.Decorator);

            containerRegistry.RegisterSingleton<IMovieDbFacade, MovieDbFacade>();
            
        }
    }
}
