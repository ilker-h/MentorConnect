using API.Extensions;

namespace API.Entities;

public class AppUser
{

    // EF will automatically know to use Id as the primary key of our database since it's a common convention. 
    // To select another property as the primary key, you'd put [Key] above it.
    public int Id { get; set; }
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public float YearsOfCareerExperience { get; set; } //
    public string KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public string MentorOrMentee { get; set; }  // this is used instead of gender. "Mentee" is used instead of "male" and "Mentor" is used instead of "female"
    public string Bio { get; set; } //
    public string LookingFor { get; set; }
    public string CareerInterests { get; set; } //
    public string City { get; set; }
    public string Country { get; set; }
    public List<Photo> Photos { get; set; } = new();

    public List<UserConnectionRequest> ConnectionRequestedByUsers { get; set; } // Connection Request is used instead of "Like"
    public List<UserConnectionRequest> ConnectionRequestedFromUsers { get; set; }

}
