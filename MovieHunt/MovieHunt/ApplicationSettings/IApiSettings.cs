namespace MovieHunt.ApplicationSettings
{
    public interface IApiSettings
    {
        string ApiKey { get; }

        string BaseUri { get; }
    }
}