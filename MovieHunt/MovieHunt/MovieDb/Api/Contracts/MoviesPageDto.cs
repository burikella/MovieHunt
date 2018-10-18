using System.Collections.Generic;
using Newtonsoft.Json;

namespace MovieHunt.MovieDb.Api.Contracts
{
    public class MoviesPageDto
    {
        public int Page { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        public IList<MovieDto> Results { get; set; }
    }
}