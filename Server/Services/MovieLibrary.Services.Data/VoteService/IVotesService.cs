namespace MovieLibrary.Services.Data.VoteService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Models;

    public interface IVotesService
    {
        Task CreateVote(CreateVoteModel createVoteModel);

        Task DeleteVote(string movieId, string userId);

        byte GetVote(string movieId, string userId);
    }
}
