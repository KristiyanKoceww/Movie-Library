namespace MovieLibrary.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MovieLibrary.Services.Data.VoteService;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;

        public VotesController(IVotesService votesService)
        {
            this.votesService = votesService;
        }

        [Authorize]
        [HttpPost("AddVote")]
        public async Task<IActionResult> Vote(CreateVoteModel model)
        {
            await this.votesService.CreateVote(model);
            return Ok();
        }

        [Authorize]
        [HttpPost("RemoveVote")]
        public async Task<IActionResult> RemoveVote(string movieId, string userId)
        {
            try
            {
                await this.votesService.DeleteVote(movieId, userId);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }

        }
        [Authorize]
        [HttpGet("GetVote")]
        public int GetVotes(string movieId, string userId)
        {
            return this.votesService.GetVote(movieId, userId);
        }
    }
}
