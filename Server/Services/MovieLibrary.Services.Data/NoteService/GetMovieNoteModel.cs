namespace MovieLibrary.Services.Data.NoteService
{
    using System.ComponentModel.DataAnnotations;

    public class GetMovieNoteModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string MovieId { get; set; }
    }
}
