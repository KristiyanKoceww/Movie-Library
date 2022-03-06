namespace MovieLibrary.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MovieLibrary.Data.Common.Models;

    public class Movie : BaseDeletableModel<string>
    {
        public Movie()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Categories = new HashSet<MovieCategory>();
            this.MovieNotes = new HashSet<MovieNote>();
            this.Votes = new HashSet<Vote>();
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Year { get; set; }

        public int Lenght { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<MovieCategory> Categories { get; set; }

        public virtual ICollection<MovieNote> MovieNotes { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
