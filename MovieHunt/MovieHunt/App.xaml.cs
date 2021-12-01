﻿using System;
using System.Collections.Generic;
using DryIoc;
using MovieHunt.ApplicationSettings;
using MovieHunt.MovieDb;
using MovieHunt.MovieDb.Api;
using MovieHunt.MovieDb.Api.Contracts;
using MovieHunt.MovieDb.Mapping;
using MovieHunt.UserInterface.ViewModels;
using MovieHunt.UserInterface.Views;
using Plugin.Connectivity;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Prism.DryIoc;

namespace MovieHunt
{
    public class ReportBuilder
    {
        private readonly List<string> _amounts = new List<string>();

        public ReportBuilder(bool includeZeroes)
        {
            IncludeZeroes = includeZeroes;
        }
        
        public bool IncludeZeroes { get; }

        public IReadOnlyList<string> Amounts => _amounts.AsReadOnly();

        public void AddAmountIfNeeded(decimal amount)
        {
            if (amount > 0 || (amount == 0 && IncludeZeroes))
            {
                _amounts.Add(amount.ToString("0.00"));
            }
            else if (amount < 0)
            {
                _amounts.Add($"({-amount:0.00})");
            }
        }
    }

    public partial class App : PrismApplication
    {
        private Lazy<AppSettings> _settings;

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null)
        {
        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            _settings = new Lazy<AppSettings>(ReadSettings);

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
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<UpcomingMoviesPage>();
            containerRegistry.RegisterForNavigation<MovieDetailsPage>();

            var container = containerRegistry.GetContainer();

            container.RegisterDelegate(r => (IApiSettings) _settings.Value);
            container.RegisterDelegate(r => (IRetrySettings) _settings.Value);

            container.UseInstance(CrossConnectivity.Current);

            container.Register<UpcomingMoviesPageViewModel>();
            container.Register<IMovieDbApiFactory, MovieDbApiFactory>(Reuse.Singleton);
            container.Register<IMovieDtoToMovieInfoMapperFactory, MovieDtoToMovieInfoMapperFactory>(Reuse.Singleton);
            container.Register<IMovieDbApi, RetryingMovieDbApi>();

            containerRegistry.RegisterSingleton<IMovieDbFacade, MovieDbFacade>();
        }

        private static AppSettings ReadSettings()
        {
            return new AppSettings
            {
                ApiKey = (string) Current.Resources["ApiKey"],
                BaseUri = (string) Current.Resources["BaseUri"],

                ApiRetryCount = (int) Current.Resources["RetryCount"],
                ApiRetryDelay = TimeSpan.FromMilliseconds((int) Current.Resources["RetryDelayMilliseconds"])
            };
        }
    }
}