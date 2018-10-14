using System;

namespace MovieHunt.ApplicationSettings
{
    public interface IRetrySettings
    {
        int ApiRetryCount { get; }

        TimeSpan ApiRetryDelay { get; }
    }
}