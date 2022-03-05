namespace MovieLibrary.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MovieLibrary.Data.Common.Models;

    public class MovieNote : BaseDeletableModel<int>
    {
        [Required]
        public string Note { get; set; }

        [Required]
        public string MovieId { get; set; }

        [Required]
        public virtual Movie Movie { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}
