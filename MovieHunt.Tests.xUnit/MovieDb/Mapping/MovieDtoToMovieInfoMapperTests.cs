using System;
using System.Collections.Generic;
using MovieHunt.MovieDb.Api.Contracts;
using MovieHunt.MovieDb.Mapping;
using Xunit;

namespace MovieHunt.Tests.xUnit.MovieDb.Mapping
{
    public class MovieDtoToMovieInfoMapperTests
    {
        [Theory]
        [MemberData(nameof(GetNullArgumentCases))]
        public void Create_NullArgument_ArgumentNullExceptionThrown(
            IDictionary<int, string> genres,
            string imagesBaseUri,
            string previewSize,
            string backdropSize)
        {
            Assert.Throws<ArgumentNullException>(
                () => new MovieDtoToMovieInfoMapper(genres, imagesBaseUri, previewSize, backdropSize));
        }

        [Theory]
        [InlineData(1, "test1")]
        [InlineData(1, "test2")]
        [InlineData(2, "test3")]
        public void Map_UsesDictionaryToMapGenres(int id, string name)
        {
            // Arrange
            var dictionary = new Dictionary<int, string> {{id, name}};
            var mapper = new MovieDtoToMovieInfoMapper(dictionary, string.Empty, string.Empty, string.Empty);

            // Act
            var result = mapper.Map(new MovieDto {GenreIds = new[] {id}});

            // Assert
            Assert.Collection(result.Genres, item => Assert.Equal(name, item));
        }

        [Theory]
        [MemberData(nameof(GetPropertySets))]
        public void Map_MapsProperties(string title, DateTime releaseDate, string overview)
        {
            // Arrange
            var mapper = new MovieDtoToMovieInfoMapper(
                new Dictionary<int, string>(), string.Empty, string.Empty, string.Empty);

            // Act
            var result = mapper.Map(new MovieDto
            {
                Title = title,
                ReleaseDate = releaseDate,
                Overview = overview
            });

            // Assert
            Assert.Equal(title, result.Title);
            Assert.Equal(releaseDate, result.ReleaseDate);
            Assert.Equal(overview, result.Overview);
        }

        [Theory]
        [InlineData("base1", "ps1", "poster", "bs1", "backdrop")]
        [InlineData("base2", "ps2", "poster2", "bs2", "backdrop2")]
        public void Map_MapsImages(
            string baseUrl, string posterSize, string poster, string backdropSize, string backdrop)
        {
            // Arrange
            var mapper = new MovieDtoToMovieInfoMapper(
                new Dictionary<int, string>(), baseUrl, posterSize, backdropSize);

            // Act
            var result = mapper.Map(new MovieDto
            {
                PosterPath = poster,
                BackdropPath = backdrop
            });

            // Assert
            Assert.Equal($"{baseUrl}{posterSize}{poster}", result.PreviewImage);
            Assert.Equal($"{baseUrl}{backdropSize}{backdrop}", result.BackdropImage);
        }

        private static IEnumerable<object[]> GetPropertySets()
        {
            yield return new object[]{"title1", new DateTime(2015, 03, 18), "overview test"};
            yield return new object[]{"title2", new DateTime(2018, 12, 05), "some text"};
        }

        private static IEnumerable<object[]> GetNullArgumentCases()
        {
            var dictionary = new Dictionary<int, string>();

            yield return new object[]{dictionary, string.Empty, string.Empty, null};
            yield return new object[]{dictionary, string.Empty, null, string.Empty};
            yield return new object[]{dictionary, null, string.Empty, string.Empty};
            yield return new object[]{null, string.Empty, string.Empty, string.Empty};
        }
    }
}