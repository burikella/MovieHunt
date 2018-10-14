using System.Threading.Tasks;

namespace MovieHunt.MovieDb
{
    internal interface IMovieDbFacade
    {
        Task<MoviesLoadingResult> LoadMovies(int page);
    }
}