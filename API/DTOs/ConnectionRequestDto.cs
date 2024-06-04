namespace API.DTOs
{
    // this will contain the properties to display inside a member card when a user is viewing a list of
    // users they've sent connection requests to
    public class ConnectionRequestDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public float YearsOfCareerExperience { get; set; }
        public string KnownAs { get; set; }
        public string PhotoUrl { get; set; }
        public string City { get; set; }
    }
}