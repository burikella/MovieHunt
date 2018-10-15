using System.Collections.Generic;
using Newtonsoft.Json;

namespace MovieHunt.MovieDb.Api.Contracts
{
    public class ImagesConfigurationDto
    {
        [JsonProperty("secure_base_url")]
        public string SecureBaseUrl { get; set; }

        [JsonProperty("backdrop_sizes")]
        public IList<string> BackdropSizes { get; set; }

        [JsonProperty("poster_sizes")]
        public IList<string> PosterSizes { get; set; }
    }
}