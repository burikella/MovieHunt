using System.Threading.Tasks;
using Refit;

namespace MovieHunt.MovieDb.Api.Contracts
{
    public interface IMovieDbApi
    {
        [Get("/movie/upcoming")]
        Task<MoviesPageDto> GetUpcomingMovies(int page);

        [Get("/genre/movie/list")]
        Task<GenresDto> GetMovieGenres();

        [Get("/configuration")]
        Task<ConfigurationDto> GetConfiguration();
    }
}