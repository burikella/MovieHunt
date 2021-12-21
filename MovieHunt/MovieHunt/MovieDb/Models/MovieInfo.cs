using System;
using System.Collections.Generic;

namespace MovieHunt.MovieDb.Models
{
    public class MovieInfo
    {
        public string Title { get; set; }

        public ICollection<string> Genres { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string PreviewImage { get; set; }

        public string BackdropImage { get; set; }

        public string Overview { get; set; }
    }
}