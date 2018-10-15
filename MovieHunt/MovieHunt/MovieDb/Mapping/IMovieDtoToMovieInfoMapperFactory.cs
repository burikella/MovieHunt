using System.Collections.Generic;

namespace MovieHunt.MovieDb.Mapping
{
    internal interface IMovieDtoToMovieInfoMapperFactory
    {
        IMovieDtoToMovieInfoMapper Create(IDictionary<int, string> genres, string imagesBaseUri, string previewSize, string backdropSize);
    }
}