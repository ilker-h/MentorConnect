namespace API.Helpers
{
    public class ConnectionRequestsParams : PaginationParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }
    }
}