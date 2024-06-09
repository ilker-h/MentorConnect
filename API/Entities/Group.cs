using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Group
    {
        public Group()
        {

        }

        public Group(string name)
        {
            Name = name;
        }

        [Key] // this is going to be the primary key in the db
        public string Name { get; set; }
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }
}