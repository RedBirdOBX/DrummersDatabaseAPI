using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrummersDatabaseAPI.Data.Entities
{
    public class Entry
    {
        public Entry()
        {
            Name = string.Empty;
            Description = string.Empty;
            Created = DateTime.Now;
            Url = string.Empty;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid EntryGuid { get; set; }

        [Required(ErrorMessage = $"{nameof(Name)} is required")]
        [MaxLength(ErrorMessage = $"Max length for is {nameof(Name)} 100.")]
        public string Name { get; set; }

        [Required(ErrorMessage = $"{nameof(Description)} is required")]
        [MaxLength(ErrorMessage = $"Max length for is {nameof(Description)} 100.")]
        public string Description { get; set; }

        [MaxLength(ErrorMessage = $"Max length for is {nameof(Url)} 500.")]
        public string Url { get; set; }

        public string Image { get; set; }

        public bool Active { get; set; }

        public int Counter { get; set; }

        public DateTime Created { get; set; }

        // parent
        public int SubCategoryId { get; set; }

        public Guid SubCategoryGuid { get; set; }

        [ForeignKey("SubCategoryId")]
        public SubCategory? SubCategory { get; set; }
    }
}
