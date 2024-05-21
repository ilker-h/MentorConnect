namespace API.Entities;

public class AppUser
{

// EF will automatically know to use Id as the primary key of our database since it's a common convention. 
// To select another property as the primary key, you'd put [Key] above it.
    public int Id { get; set; } 
    
    public string UserName { get; set; }
    
    public byte[] PasswordHash { get; set; }
    
    public byte[] PasswordSalt { get; set; }
}
