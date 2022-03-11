namespace MovieLibrary.Services.Data.MovieService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
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

        public async Task AddMovieToFavourites(AddMovieToFavModel movieModel)
        {
            var movieExist = this.movieRepository.All().Where(x => x.Title == movieModel.Title).FirstOrDefault();
            var user = this.appUsersRepository.All().Where(x => x.Id == movieModel.UserId).FirstOrDefault();
            if (user is null)
            {
                throw new Exception("No user found.");
            }

            if (movieExist is null)
            {
                var movie = new Movie()
                {
                    Title = movieModel.Title,
                    ImageUrl = movieModel.ImageUrl,
                    Lenght = movieModel.Lenght,
                    Year = movieModel.Year,
                    Description = movieModel.Description,
                };

                user.Movies.Add(movie);

                await this.movieRepository.AddAsync(movie);
                await this.movieRepository.SaveChangesAsync();
                await this.appUsersRepository.SaveChangesAsync();
            }
            else
            {
                if (user.Movies.Any(x => x.Title == movieModel.Title))
                {
                    throw new Exception("This movie is already added to your collection");
                }

                user.Movies.Add(movieExist);
                await this.appUsersRepository.SaveChangesAsync();
            }
        }

        public IEnumerable<Movie> GetMovies(string userId)
        {
            var user = this.appUsersRepository.All().Where(x => x.Id == userId).Include(x => x.Movies).Take(5).FirstOrDefault();

            if (user is null)
            {
                throw new Exception("No user found.");
            }

            if (user.Movies.Count == 0)
            {
                throw new Exception("There are no movies added in favourites.");
            }

            return user.Movies;
        }

        public async Task RemoveMovieFromFavourites(string movieId, string userId)
        {
            var user = this.appUsersRepository.All().Where(x => x.Id == userId).Include(x => x.Movies).FirstOrDefault();

            if (user is null)
            {
                throw new Exception("No user found.");
            }

            if (user.Movies.Count == 0)
            {
                throw new Exception("There are no movies added in favourites.");
            }

            var movie = user.Movies.Where(x => x.Id == movieId).FirstOrDefault();

            if (movie is not null)
            {
                user.Movies.Remove(movie);

                this.appUsersRepository.Update(user);
                this.movieRepository.HardDelete(movie);

                await this.movieRepository.SaveChangesAsync();
                await this.appUsersRepository.SaveChangesAsync();
            }
            else
            {
                throw new Exception("No movie found by this id.");
            }
        }
    }
}
