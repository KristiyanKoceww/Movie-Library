namespace MovieLibrary.Services.Data.NoteService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MovieLibrary.Data.Models;

    public interface INoteService
    {
        Task CreateNote(CreateNoteModel createNoteModel);

        Task DeleteNote(DeleteMovieNoteModel deleteMovieNoteModel);

        IEnumerable<MovieNote> GetNotes(string userId , string movieId);
    }
}
