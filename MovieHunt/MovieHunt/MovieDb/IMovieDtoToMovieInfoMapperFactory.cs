using System.Collections.Generic;

namespace MovieHunt.MovieDb
{
    internal interface IMovieDtoToMovieInfoMapperFactory
    {
        IMovieDtoToMovieInfoMapper Create(IDictionary<int, string> genres, string imagesBaseUri, string previewSize, string backdropSize);
    }
}