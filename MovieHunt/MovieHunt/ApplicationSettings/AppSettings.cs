using System;

namespace MovieHunt.ApplicationSettings
{
    public class AppSettings : IRetrySettings, IApiSettings
    {
        public int ApiRetryCount { get; set; }

        public TimeSpan ApiRetryDelay { get; set; }

        public string ApiKey { get; set; }

        public string BaseUri { get; set; }

        public string ApplicationName { get; set; }
    }
}