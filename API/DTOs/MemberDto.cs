namespace API.DTOs
{
    public class MemberDto
    {
    public int Id { get; set; }
    public string UserName { get; set; }
    public string PhotoUrl { get; set; }
    public int Age { get; set; }
    public float YearsOfCareerExperience { get; set; } //
    public string KnownAs { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastActive { get; set; }
    public string MentorOrMentee { get; set; } //
    public string Bio { get; set; } //
    public string LookingFor { get; set; }
    public string CareerInterests { get; set; } //
    public string City { get; set; }
    public string Country { get; set; }
    public List<PhotoDto> Photos { get; set; }
    }
}