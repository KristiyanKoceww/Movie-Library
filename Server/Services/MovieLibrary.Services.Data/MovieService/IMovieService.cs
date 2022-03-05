namespace MovieLibrary.Services.Data.MovieService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Models;

    public interface IMovieService
    {
        Task AddMovieToFavourites(AddMovieToFavModel movie, string userId);

        Task RemoveMovieFromFavourites(string movieId, string userId);

        IEnumerable<Movie> GetMovies(string userId);
    }
}
