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

        //[Authorize]
        [HttpPost("AddNote")]
        public async Task<IActionResult> AddMovieNote([FromForm] CreateNoteModel createNoteModel)
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

        //[Authorize]
        [HttpPost("RemoveNote")]
        public async Task<IActionResult> RemoveMovieNote([FromForm] DeleteMovieNoteModel deleteMovieNoteModel)
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

        //[Authorize]
        [HttpPost("GetMovieNote")]
        public async Task<IActionResult> GetMovieNote([FromForm] GetMovieNoteModel getMovieNoteModel)
        {
            try
            {
                var notes = this.noteService.GetNotes(getMovieNoteModel);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
