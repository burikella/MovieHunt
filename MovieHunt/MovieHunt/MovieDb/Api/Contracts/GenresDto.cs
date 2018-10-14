using System.Collections.Generic;
using MovieHunt.MovieDb.Contract;

namespace MovieHunt.MovieDb.Api.Contracts
{
    public class GenresDto
    {
        public ICollection<GenreDto> Genres { get; set; }
    }
}