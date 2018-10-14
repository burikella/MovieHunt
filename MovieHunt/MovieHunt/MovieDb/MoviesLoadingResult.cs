using System.Collections.Generic;
using MovieHunt.UserInterface.ViewModels;

namespace MovieHunt.MovieDb
{
    internal class MoviesLoadingResult
    {
        public MoviesLoadingResult(int pageNumber, int totalPages, IList<MovieInfo> movies)
        {
            PageNumber = pageNumber;
            TotalPages = totalPages;
            Movies = movies;
        }

        public int PageNumber { get; }

        public int TotalPages { get; }

        public IList<MovieInfo> Movies { get; }
    }
}