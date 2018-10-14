using System.Collections.Generic;
using Newtonsoft.Json;

namespace MovieHunt.MovieDb.Api.Contracts
{
    public class MovieDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("genre_ids")]
        public ICollection<int> GenreIds { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }
    }
}