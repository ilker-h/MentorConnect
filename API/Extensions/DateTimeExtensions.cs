namespace API.Extensions
{
    // extensions must be static classes
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly dob)
        {
            // this is a simple age calculation that doesn't take into account complexities like leap years
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var age = today.Year - dob.Year;

            if (dob > today.AddYears(-age)) age--; // if they haven't had their birthday yet this year

            return age;
        }
    }
}