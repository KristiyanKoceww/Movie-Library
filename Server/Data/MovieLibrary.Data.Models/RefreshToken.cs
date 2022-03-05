namespace MovieLibrary.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RefreshToken
    {
        [Key]
        public string Token { get; set; }

        public string UserId { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
