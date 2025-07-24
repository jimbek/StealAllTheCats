using StealAllTheCats.API.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [NotMapped]
        public string Url { get; set; } = string.Empty;

        [NotMapped]
        public virtual IList<TagEntity> TagEntities { get; set; } = [];

        public CatEntity() { }

        public CatEntity(Image image)
        {
            CatId = image.id;
            Width = image.width;
            Height = image.height;
            Image = image.GetImageName();

            if (image.breeds != null)
            {
                foreach (var breed in image.breeds)
                {
                    if (!string.IsNullOrWhiteSpace(breed.temperament))
                    {
                        string[] temperaments = breed.temperament.Split(", ");

                        foreach (string temperament in temperaments)
                        {
                            TagEntities.Add(new TagEntity
                            {
                                Name = temperament
                            });
                        }
                    }
                }
            }
        }
    }
}
