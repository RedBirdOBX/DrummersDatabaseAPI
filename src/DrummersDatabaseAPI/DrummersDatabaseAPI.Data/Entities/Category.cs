using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrummersDatabaseAPI.Data.Entities
{
    public class Category
    {
        public Category()
        {
            Name = string.Empty;
            Image = string.Empty;
            Created = DateTime.Now;
            SubCategories = new List<SubCategory>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid CategoryGuid { get; set; }

        [Required(ErrorMessage = $"{nameof(Name)} is required")]
        [MaxLength(ErrorMessage = $"Max length for is {nameof(Name)} 100.")]
        public string Name { get; set; }

        [Required(ErrorMessage = $"{nameof(Image)} is required")]
        [MaxLength(ErrorMessage = $"Max length for is {nameof(Image)} 100.")]
        public string Image { get; set; }

        public bool Active { get; set; }

        public int Counter { get; set; }

        public DateTime Created { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
