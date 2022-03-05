namespace MovieLibrary.Services.Data.MovieService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;

    public class MovieService : IMovieService
    {
        private readonly IDeletableEntityRepository<Movie> movieRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> appUsersRepository;

        public MovieService(
            IDeletableEntityRepository<Movie> movieRepository,
            IDeletableEntityRepository<ApplicationUser> appUsersRepository)
        {
            this.movieRepository = movieRepository;
            this.appUsersRepository = appUsersRepository;
        }

        public async Task AddMovieToFavourites(AddMovieToFavModel movieModel, string userId)
        {
            var movieExist = this.movieRepository.All().Where(x => x.Title == movieModel.Title).FirstOrDefault();

            if (movieExist is not null)
            {
                throw new Exception("This movie is already added to your collection");
            }

            var movie = new Movie()
            {
                Title = movieModel.Title,
                ImageUrl = movieModel.ImageUrl,
                Lenght = movieModel.Lenght,
                Year = movieModel.Year,
                Description = movieModel.Description,
            };

            var user = this.appUsersRepository.All().Where(x => x.Id == userId).FirstOrDefault();

            if (user is null)
            {
                throw new Exception("No user found.");
            }

            user.Movies.Add(movie);

            await this.movieRepository.AddAsync(movie);
            await this.movieRepository.SaveChangesAsync();
            await this.appUsersRepository.SaveChangesAsync();
        }

        public IEnumerable<Movie> GetMovies(string userId)
        {
            var user = this.appUsersRepository.All().Where(x => x.Id == userId).FirstOrDefault();

            if (user is null)
            {
                throw new Exception("No user found.");
            }

            if (user.Movies.Count == 0)
            {
                throw new Exception("There are no movies added in favourites.");
            }

            var movies = user.Movies.Select(x => new Movie()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Lenght = x.Lenght,
                Year = x.Lenght,
                ImageUrl = x.ImageUrl,
                MovieNotes = x.MovieNotes,
                Categories = x.Categories,
                Votes = x.Votes,
            }).ToList();

            return movies;
        }

        public async Task RemoveMovieFromFavourites(string movieId, string userId)
        {
            var user = this.appUsersRepository.All().Where(x => x.Id == userId).FirstOrDefault();

            if (user is null)
            {
                throw new Exception("No user found.");
            }

            if (user.Movies.Count == 0)
            {
                throw new Exception("There are no movies added in favourites.");
            }

            var movie = user.Movies.Where(x => x.Id == movieId).FirstOrDefault();

            if (movie is null)
            {
                throw new Exception("No movie found by this id.");
            }

            user.Movies.Remove(movie);

            await this.appUsersRepository.SaveChangesAsync();
        }
    }
}
