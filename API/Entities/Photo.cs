using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    // EF will create a table called "Photos" (or just the same as the class name if this line wasn't included).
    // EF will also set up the relationship between this table and the AppUser table
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

        // this is to fully define the relationship
        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#conventions
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}