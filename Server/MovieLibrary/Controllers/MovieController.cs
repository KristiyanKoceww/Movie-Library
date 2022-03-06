namespace MovieLibrary.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    using MovieLibrary.Services.Data.MovieService;
    using Newtonsoft.Json;

    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService movieService;
        public MovieController(IMovieService movieService)
        {
            this.movieService = movieService;
        }

        [Authorize]
        [HttpPost("AddToFavourites")]
        public async Task<IActionResult> AddMovieToFavourites([FromForm] AddMovieToFavModel addMovieToFavModel)
        {
            try
            {
                await this.movieService.AddMovieToFavourites(addMovieToFavModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("RemoveFromFavourites")]
        public async Task<IActionResult> RemoveMovieFromFavourites([FromForm] string movieId, string userId)
        {
            try
            {
                await this.movieService.RemoveMovieFromFavourites(movieId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("GetFavourites")]
        public string GetFavourites([FromForm] string userId)
        {
            try
            {
                var movies = this.movieService.GetMovies(userId);
                var jsonResult = JsonConvert.SerializeObject(movies);
                return jsonResult;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
