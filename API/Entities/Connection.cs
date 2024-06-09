namespace API.Entities
{
    public class Connection
    {
        // >ust also provide an empty constructor for Entity Framework despite having another constructor so that EF isn't also expected to pass the connectionId as well.
        // This empty constructor will allow EF to not have issues with creating the migration and schema for the db
        public Connection()
        {
            
        }
        public Connection(string connectionId, string username)
        {
            ConnectionId = connectionId;
            Username = username;
        }

        public string ConnectionId { get; set; }
        public string Username { get; set; }
    }
}