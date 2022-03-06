namespace MovieLibrary.Services.Data.MovieService
{
    using System.ComponentModel.DataAnnotations;

    public class AddMovieToFavModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Year { get; set; }

        public int Lenght { get; set; }

        public string ImageUrl { get; set; }

        public string UserId { get; set; }
    }
}
