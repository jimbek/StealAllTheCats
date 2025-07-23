namespace StealAllTheCats.API.Models
{
    public class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
