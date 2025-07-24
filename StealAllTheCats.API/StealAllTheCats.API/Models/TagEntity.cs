using System.ComponentModel.DataAnnotations;

namespace StealAllTheCats.API.Models
{
    public class TagEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
