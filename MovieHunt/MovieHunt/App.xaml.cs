using System;
using DryIoc;
using MovieHunt.ApplicationSettings;
using MovieHunt.MovieDb;
using MovieHunt.MovieDb.Api;
using MovieHunt.MovieDb.Api.Contracts;
using MovieHunt.MovieDb.Mapping;
using MovieHunt.UserInterface.ViewModels;
using MovieHunt.UserInterface.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.DryIoc;

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

                ApiKey = "1f54bd990f1cdfb230adb312546d765d",
                BaseUri = "https://api.themoviedb.org/3",

                ApiRetryCount = 3,
                ApiRetryDelay = TimeSpan.FromMilliseconds(250)
            };


            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<UpcomingMoviesPage>();
            containerRegistry.RegisterForNavigation<MovieDetailsPage>();

            var container = containerRegistry.GetContainer();

            container.UseInstance<IApiSettings>(settings);
            container.UseInstance<IRetrySettings>(settings);

            container.Register<UpcomingMoviesPageViewModel>();
            container.Register<IMovieDbApiFactory, MovieDbApiFactory>(Reuse.Singleton);
            container.Register<IMovieDtoToMovieInfoMapperFactory, MovieDtoToMovieInfoMapperFactory>(Reuse.Singleton);
            container.Register<IMovieDbApi, RetryingMovieDbApi>();

            containerRegistry.RegisterSingleton<IMovieDbFacade, MovieDbFacade>();
            
        }
    }
}
