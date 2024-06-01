using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto // examples of validators are [Required], [Phone], [MaxLength], [String], [RegularExpression]
    {
        [Required]
        public string Username { get; set; }

        [Required] public string KnownAs { get; set; }
        [Required] public string MentorOrMentee { get; set; }
        [Required] public float YearsOfCareerExperience { get; set; }
        [Required] public DateOnly? DateOfBirth { get; set; } // ? optional to make [required] validator work
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}