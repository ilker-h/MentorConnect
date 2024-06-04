namespace API.Entities
{
    // Connection Request is used instead of "Like"
    public class UserConnectionRequest
    {
        // this will act as a join table
        public AppUser SourceUser { get; set; }
        public int SourceUserId { get; set; }
        public AppUser TargetUser { get; set; } // the TargetUser is sent a connection request from the SourceUser
        public int TargetUserId { get; set; }
    }
}