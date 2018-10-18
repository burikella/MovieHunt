using System.Collections.Generic;

namespace MovieHunt.MovieDb.Mapping
{
    internal class MovieDtoToMovieInfoMapperFactory : IMovieDtoToMovieInfoMapperFactory
    {
        public IMovieDtoToMovieInfoMapper Create(
            IDictionary<int, string> genres,
            string imagesBaseUri,
            string previewSize,
            string backdropSize)
        {
            return new MovieDtoToMovieInfoMapper(genres, imagesBaseUri, previewSize, backdropSize);
        }
    }
}