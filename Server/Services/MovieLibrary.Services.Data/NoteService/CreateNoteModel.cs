namespace MovieLibrary.Services.Data.NoteService
{
    using System.ComponentModel.DataAnnotations;

    public class CreateNoteModel
    {
        [Required]
        public string Note { get; set; }

        [Required]
        public string MovieId { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
