namespace MovieLibrary.Services.Data.MovieService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class RemoveMovieFromFavModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string MovieId { get; set; }
    }
}
