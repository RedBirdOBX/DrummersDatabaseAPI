using System.ComponentModel.DataAnnotations;

namespace DrummersDatabaseAPI.Dtos;

/// <summary>
/// Dto for creating Category
/// </summary>
public class CategoryCreateDto
{
    /// <summary>
    /// default constructor
    /// </summary>
    public CategoryCreateDto()
    {
        Name = string.Empty;
        Image = string.Empty;
        Active = true;
        CategoryGuid = Guid.NewGuid();
    }

    /// <summary>
    /// Category Guid
    /// </summary>
    public Guid CategoryGuid { get; set; }

    /// <summary>
    /// Category Name
    /// </summary>
    [Required(ErrorMessage = $"{nameof(Name)} is required")]
    [MaxLength(100, ErrorMessage = $"Max length for is {nameof(Name)} 100.")]
    public string Name { get; set; }

    /// <summary>
    /// Category Image
    /// </summary>
    [Required(ErrorMessage = $"{nameof(Image)} is required")]
    [MaxLength(100, ErrorMessage = $"Max length for is {nameof(Image)} 100.")]
    public string Image { get; set; }

    /// <summary>
    /// Active Flag
    /// </summary>
    public bool Active { get; set; }
}