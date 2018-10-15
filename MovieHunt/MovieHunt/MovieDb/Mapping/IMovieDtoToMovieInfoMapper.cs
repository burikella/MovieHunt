using MovieHunt.MovieDb.Api.Contracts;
using MovieHunt.MovieDb.Models;
using MovieHunt.UserInterface.ViewModels;

namespace MovieHunt.MovieDb
{
    internal interface IMovieDtoToMovieInfoMapper
    {
        MovieInfo Map(MovieDto dto);
    }
}