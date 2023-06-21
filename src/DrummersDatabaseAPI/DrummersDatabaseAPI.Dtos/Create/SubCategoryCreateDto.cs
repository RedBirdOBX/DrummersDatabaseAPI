using System.ComponentModel.DataAnnotations;

namespace DrummersDatabaseAPI.Dtos;

/// <summary>
/// Dto for creating SubCategory
/// </summary>
public class SubCategoryCreateDto
{
    /// <summary>
    /// default constructor
    /// </summary>
    public SubCategoryCreateDto()
    {
        Name = string.Empty;
        Image = string.Empty;
        Active = true;
        SubCategoryGuid = Guid.NewGuid();
    }

    /// <summary>
    /// Guid for SubCategory
    /// </summary>
    public Guid SubCategoryGuid { get; set; }

    /// <summary>
    /// CategoryId
    /// </summary>
    [Required(ErrorMessage = $"{nameof(CategoryId)} is required")]
    public int CategoryId { get; set; }

    /// <summary>
    /// Category Guid
    /// </summary>
    [Required(ErrorMessage = $"{nameof(CategoryGuid)} is required")]
    public Guid CategoryGuid { get; set; }

    /// <summary>
    /// Name of SubCategory
    /// </summary>
    [Required(ErrorMessage = $"{nameof(Name)} is required")]
    [MaxLength(100, ErrorMessage = $"Max length for is {nameof(Name)} 100.")]
    public string Name { get; set; }

    /// <summary>
    /// SubCategory Image
    /// </summary>
    [Required(ErrorMessage = $"{nameof(Image)} is required")]
    [MaxLength(100, ErrorMessage = $"Max length for is {nameof(Image)} 100.")]
    public string Image { get; set; }

    /// <summary>
    /// Active flag
    /// </summary>
    public bool Active { get; set; }
}
