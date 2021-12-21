using MovieHunt.MovieDb.Api.Contracts;
using MovieHunt.MovieDb.Models;

namespace MovieHunt.MovieDb.Mapping
{
    internal interface IMovieDtoToMovieInfoMapper
    {
        MovieInfo Map(MovieDto dto);
    }
}