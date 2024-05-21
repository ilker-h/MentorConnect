using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto // examples of validators are [Required], [Phone], [MaxLength], [String], [RegularExpression]
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}