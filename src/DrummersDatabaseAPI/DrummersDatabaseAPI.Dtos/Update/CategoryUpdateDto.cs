using System.ComponentModel.DataAnnotations;

namespace DrummersDatabaseAPI.Dtos;

/// <summary>
/// Dto for updating Category
/// </summary>
public class CategoryUpdateDto
{
    /// <summary>
    /// default constructor
    /// </summary>
    public CategoryUpdateDto()
    {
        Name = string.Empty;
        Image = string.Empty;
    }

    /// <summary>
    /// Name of Category
    /// </summary>
    [Required(ErrorMessage = $"{nameof(Name)} is required")]
    [MaxLength(100, ErrorMessage = $"Max length for is {nameof(Name)} 100.")]
    public string Name { get; set; }

    /// <summary>
    /// Image of Category
    /// </summary>
    [Required(ErrorMessage = $"{nameof(Image)} is required")]
    [MaxLength(100, ErrorMessage = $"Max length for is {nameof(Image)} 100.")]
    public string Image { get; set; }

    /// <summary>
    /// Active Flag
    /// </summary>
    public bool Active { get; set; }
}