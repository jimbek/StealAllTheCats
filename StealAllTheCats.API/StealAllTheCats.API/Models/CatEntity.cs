using StealAllTheCats.API.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace StealAllTheCats.API.Models
{
    public class CatEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string CatId { get; set; } = string.Empty;

        public int Width { get; set; }

        public int Height { get; set; }

        [Required]
        [MaxLength(32)]
        public string Image { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public CatEntity() { }

        public CatEntity(Image image)
        {
            CatId = image.id;
            Width = image.width;
            Height = image.height;
            Image = image.GetImageName();
        }
    }
}
