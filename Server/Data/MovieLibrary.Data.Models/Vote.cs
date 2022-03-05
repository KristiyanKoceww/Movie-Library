namespace MovieLibrary.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MovieLibrary.Data.Common.Models;

    public class Vote : BaseModel<int>
    {
        [Required]
        public byte Value { get; set; }

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
