using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrummersDatabaseAPI.Data.Entities
{
    public class SubCategory
    {
        public SubCategory()
        {
            Name = string.Empty;
            Image = string.Empty;
            Created = DateTime.Now;
            Entries = new List<Entry>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid SubCategoryGuid { get; set; }

        [Required(ErrorMessage = $"{nameof(Name)} is required")]
        [MaxLength(ErrorMessage = $"Max length for is {nameof(Name)} 100.")]
        public string Name { get; set; }

        [Required(ErrorMessage = $"{nameof(Image)} is required")]
        [MaxLength(ErrorMessage = $"Max length for is {nameof(Image)} 100.")]
        public string Image { get; set; }

        public bool Active { get; set; }

        public int Counter { get; set; }

        public DateTime Created { get; set; }

        public ICollection<Entry> Entries { get; set; }

        // parent
        [Required(ErrorMessage = $"{nameof(CategoryId)} is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = $"{nameof(CategoryGuid)} is required")]
        public Guid CategoryGuid { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}
