namespace API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10; // _ is the convention for naming private variables

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string CurrentUsername { get; set; }
        public string MentorOrMentee { get; set; }
        public int MinYearsOfCareerExperience { get; set; } = 0; // MinAge
        public int MaxYearsOfCareerExperience { get; set; } = 50; // MinAge
        public string OrderBy { get; set; } = "lastActive"; // the default sorting
    }
}