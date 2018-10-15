using System;
using System.Collections.Generic;
using System.Linq;
using MovieHunt.MovieDb.Api.Contracts;
using MovieHunt.UserInterface.ViewModels;

namespace MovieHunt.MovieDb
{
    internal class MovieDtoToMovieInfoMapper : IMovieDtoToMovieInfoMapper
    {
        private readonly IDictionary<int, string> _genres;
        private readonly string _imagesBaseUri;
        private readonly string _previewSize;
        private readonly string _backdropSize;

        public MovieDtoToMovieInfoMapper(
            IDictionary<int, string> genres,
            string imagesBaseUri,
            string previewSize,
            string backdropSize)
        {
            _genres = genres ?? throw new ArgumentNullException(nameof(genres));
            _imagesBaseUri = imagesBaseUri ?? throw new ArgumentNullException(nameof(imagesBaseUri));
            _previewSize = previewSize ?? throw new ArgumentNullException(nameof(previewSize));
            _backdropSize = backdropSize ?? throw new ArgumentNullException(nameof(backdropSize));
        }

        public MovieInfo Map(MovieDto dto)
        {
            return new MovieInfo
            {
                Title = dto.Title,
                Genres = dto.GenreIds?.Select(id => _genres[id]).ToList(),
                ReleaseDate = dto.ReleaseDate,
                PreviewImage = $"{_imagesBaseUri}{_previewSize}{dto.PosterPath}",
                BackdropImage = $"{_imagesBaseUri}{_backdropSize}{dto.BackdropPath}",
                Overview = dto.Overview
            };
        }
    }
}