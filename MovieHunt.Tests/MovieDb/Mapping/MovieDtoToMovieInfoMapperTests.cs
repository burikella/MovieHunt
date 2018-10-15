using System;
using System.Collections.Generic;
using FluentAssertions;
using MovieHunt.MovieDb.Api.Contracts;
using MovieHunt.MovieDb.Mapping;
using NUnit.Framework;

namespace MovieHunt.Tests.MovieDb.Mapping
{
    [TestFixture]
    public class MovieDtoToMovieInfoMapperTests
    {
        [TestCaseSource(nameof(GetNullArgumentCases))]
        public void Create_NullArgument_ArgumentNullExceptionThrown(
            IDictionary<int, string> genres,
            string imagesBaseUri,
            string previewSize,
            string backdropSize)
        {
            Assert.Throws<ArgumentNullException>(
                () => new MovieDtoToMovieInfoMapper(genres, imagesBaseUri, previewSize, backdropSize));
        }

        [TestCase(1, "test1")]
        [TestCase(1, "test2")]
        [TestCase(2, "test3")]
        public void Map_UsesDictionaryToMapGenres(int id, string name)
        {
            // Arrange
            var dictionary = new Dictionary<int, string> {{id, name}};
            var mapper = new MovieDtoToMovieInfoMapper(dictionary, string.Empty, string.Empty, string.Empty);

            // Act
            var result = mapper.Map(new MovieDto {GenreIds = new[] {id}});

            // Assert
            result.Genres.Should().BeEquivalentTo(name);
        }

        [TestCaseSource(nameof(GetPropertySets))]
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
            result.Should().BeEquivalentTo(new
            {
                Title = title,
                ReleaseDate = releaseDate,
                Overview = overview
            });
        }

        [TestCase("base1", "ps1", "poster", "bs1", "backdrop")]
        [TestCase("base2", "ps2", "poster2", "bs2", "backdrop2")]
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
            result.Should().BeEquivalentTo(new
            {
                PreviewImage = $"{baseUrl}{posterSize}{poster}",
                BackdropImage = $"{baseUrl}{backdropSize}{backdrop}"
            });
        }

        private static IEnumerable<TestCaseData> GetPropertySets()
        {
            yield return new TestCaseData("title1", new DateTime(2015, 03, 18), "overview test");
            yield return new TestCaseData("title2", new DateTime(2018, 12, 05), "some text");
        }

        private static IEnumerable<TestCaseData> GetNullArgumentCases()
        {
            var dictionary = new Dictionary<int, string>();

            yield return new TestCaseData(dictionary, string.Empty, string.Empty, null);
            yield return new TestCaseData(dictionary, string.Empty, null, string.Empty);
            yield return new TestCaseData(dictionary, null, string.Empty, string.Empty);
            yield return new TestCaseData(null, string.Empty, string.Empty, string.Empty);
        }
    }
}