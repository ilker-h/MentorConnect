namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        public string CurrentUsername { get; set; }
        public string MentorOrMentee { get; set; }
        public int MinYearsOfCareerExperience { get; set; } = 0; // MinAge
        public int MaxYearsOfCareerExperience { get; set; } = 50; // MinAge
        public string OrderBy { get; set; } = "lastActive"; // the default sorting
    }
}