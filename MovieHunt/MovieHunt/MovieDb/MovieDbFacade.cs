using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieHunt.MovieDb.Api.Contracts;
using MovieHunt.MovieDb.Mapping;
using MovieHunt.MovieDb.Models;
using MovieHunt.Utility;

namespace MovieHunt.MovieDb
{
    internal class MovieDbFacade : IMovieDbFacade
    {
        private readonly IMovieDtoToMovieInfoMapperFactory _mapperFactory;
        private readonly IMovieDbApi _api;

        private readonly AsyncLazy<IMovieDtoToMovieInfoMapper> _mapper;

        public MovieDbFacade(IMovieDbApi api, IMovieDtoToMovieInfoMapperFactory mapperFactory)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _mapperFactory = mapperFactory ?? throw new ArgumentNullException(nameof(mapperFactory));
            _mapper = new AsyncLazy<IMovieDtoToMovieInfoMapper>(CreateMapper);
        }

        private async Task<IMovieDtoToMovieInfoMapper> CreateMapper()
        {
            var genres =  await FetchGenreNames();
            var configuration = await _api.GetConfiguration();

            return _mapperFactory.Create(
                genres,
                configuration.Images.SecureBaseUrl,
                configuration.Images.PosterSizes.First(),
                configuration.Images.BackdropSizes.Last());
        }

        public async Task<MoviesLoadingResult> LoadMovies(int page)
        {
            var result = await _api.GetUpcomingMovies(page);
            var mapper = await _mapper.Value;

            var movies = result.Results?.Select(mapper.Map) ?? Array.Empty<MovieInfo>();

            return new MoviesLoadingResult(
                result.Page,
                result.TotalPages,
                movies.ToList());
        }

        private async Task<IDictionary<int, string>> FetchGenreNames()
        {
            var result = await _api.GetMovieGenres();
            return result.Genres.ToDictionary(g => g.Id, g => g.Name);
        }
    }
}