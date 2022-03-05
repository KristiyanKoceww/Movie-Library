namespace MovieLibrary.Services.Data.VoteService
{
    using System.ComponentModel.DataAnnotations;

    public class CreateVoteModel
    {
        [Required]
        public string MovieId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Range(0, 5)]
        public byte Value { get; set; }
    }
}
