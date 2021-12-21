using MovieHunt.MovieDb.Api.Contracts;

namespace MovieHunt.MovieDb.Api
{
    internal interface IMovieDbApiFactory
    {
        IMovieDbApi Create();
    }
}