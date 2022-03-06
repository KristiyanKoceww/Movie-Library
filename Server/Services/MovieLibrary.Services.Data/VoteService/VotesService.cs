namespace MovieLibrary.Services.Data.VoteService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using MovieLibrary.Data.Common.Repositories;
    using MovieLibrary.Data.Models;

    public class VotesService : IVotesService
    {
        private readonly IDeletableEntityRepository<Movie> moviesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> appUsersRepository;
        private readonly IDeletableEntityRepository<Vote> votesRepository;

        public VotesService(
            IDeletableEntityRepository<Movie> moviesRepository,
            IDeletableEntityRepository<ApplicationUser> appUsersRepository,
            IDeletableEntityRepository<Vote> votesRepository)
        {
            this.moviesRepository = moviesRepository;
            this.appUsersRepository = appUsersRepository;
            this.votesRepository = votesRepository;
        }

        public async Task CreateVote(CreateVoteModel createVoteModel)
        {
            var vote = this.votesRepository.All()
                 .FirstOrDefault(x => x.MovieId == createVoteModel.MovieId && x.UserId == createVoteModel.UserId);

            if (vote == null)
            {
                vote = new Vote()
                {
                    MovieId = createVoteModel.MovieId,
                    UserId = createVoteModel.UserId,
                    Value = createVoteModel.Value,
                };

                await this.votesRepository.AddAsync(vote);
            }

            vote.Value = createVoteModel.Value;
            await this.votesRepository.SaveChangesAsync();
        }

        public async Task DeleteVote(string movieId, string userId)
        {
            var vote = this.votesRepository.All()
                 .FirstOrDefault(x => x.MovieId == movieId && x.UserId == userId);

            var movie = this.moviesRepository.All().Where(x => x.Id == movieId).Include(x => x.Votes).FirstOrDefault();

            if (vote is null)
            {
                throw new Exception("No vote found");
            }

            if (movie is null)
            {
                throw new Exception("No movie found");
            }

            var movieVote = movie.Votes.Where(x => x.Id == vote.Id).FirstOrDefault();
            if (movieVote is not null)
            {
                movie.Votes.Remove(movieVote);

                this.moviesRepository.Update(movie);
                this.votesRepository.HardDelete(vote);

                await this.moviesRepository.SaveChangesAsync();
                await this.votesRepository.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Deleting went wrong.");
            }
        }

        public byte GetVote(string movieId, string userId)
        {
            var movie = this.moviesRepository.All().Where(x => x.Id == movieId).Include(x => x.Votes).FirstOrDefault();

            if (movie is not null)
            {
                var vote = movie.Votes.Where(x => x.UserId == userId).FirstOrDefault();

                if (vote is not null)
                {
                    return vote.Value;
                }
            }

            return 0;
        }
    }
}
