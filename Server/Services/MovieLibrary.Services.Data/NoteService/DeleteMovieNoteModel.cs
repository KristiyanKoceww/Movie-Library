namespace MovieLibrary.Services.Data.NoteService
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteMovieNoteModel
    {
        [Required]
        public string MovieId { get; set; }

        [Required]
        public int NoteId { get; set; }
    }
}
