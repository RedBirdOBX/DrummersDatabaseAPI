using System.ComponentModel.DataAnnotations;

namespace DrummersDatabaseAPI.Dtos;

/// <summary>
/// Dto to update SubCategory
/// </summary>
public class SubCategoryUpdateDto
{
    /// <summary>
    /// default constructor
    /// </summary>
    public SubCategoryUpdateDto()
    {
        Name = string.Empty;
        Image = string.Empty;
    }

    /// <summary>
    /// CategoryId
    /// </summary>
    [Required(ErrorMessage = $"{nameof(CategoryId)} is required")]
    public int CategoryId { get; set; }

    /// <summary>
    /// CategoryGuid
    /// </summary>
    [Required(ErrorMessage = $"{nameof(CategoryGuid)} is required")]
    public Guid CategoryGuid { get; set; }

    /// <summary>
    /// SubCategory Name
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
    /// Active Flag
    /// </summary>
    public bool Active { get; set; }
}
