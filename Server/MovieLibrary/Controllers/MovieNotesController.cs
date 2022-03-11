using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Services.Data.NoteService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieLibrary.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieNotesController : ControllerBase
    {
        private readonly INoteService noteService;
        public MovieNotesController(INoteService noteService)
        {
            this.noteService = noteService;
        }

        [Authorize]
        [HttpPost("AddNote")]
        public async Task<IActionResult> AddMovieNote([FromBody] CreateNoteModel createNoteModel)
        {
            try
            {
                await this.noteService.CreateNote(createNoteModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("RemoveNote")]
        public async Task<IActionResult> RemoveMovieNote([FromBody] DeleteMovieNoteModel deleteMovieNoteModel)
        {
            try
            {
                await this.noteService.DeleteNote(deleteMovieNoteModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("GetMovieNote")]
        public async Task<IActionResult> GetMovieNote(string movieId, string userId)
        {
            try
            {
                var model = new GetMovieNoteModel()
                {
                    MovieId = movieId,
                    UserId = userId
                };

                var notes = this.noteService.GetNotes(model);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
