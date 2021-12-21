using System.Collections.Generic;

namespace MovieHunt.MovieDb.Api.Contracts
{
    public class GenresDto
    {
        public ICollection<GenreDto> Genres { get; set; }
    }
}