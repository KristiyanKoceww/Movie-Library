namespace MovieLibrary.Services.Data.VoteService
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task CreateVote(CreateVoteModel createVoteModel);

        Task DeleteVote(string movieId, string userId);

        byte GetVote(string movieId, string userId);
    }
}
