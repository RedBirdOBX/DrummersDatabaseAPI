using System.ComponentModel.DataAnnotations;

namespace DrummersDatabaseAPI.Dtos;

/// <summary>
/// Dto to update Entry
/// </summary>
public class EntryUpdateDto
{
    /// <summary>
    /// default constructor
    /// </summary>
    public EntryUpdateDto()
    {
        Name = string.Empty;
        Description = string.Empty;
        Url = string.Empty;
        Image = string.Empty;
    }

    /// <summary>
    /// SubCategoryId
    /// </summary>
    [Required(ErrorMessage = "SubCategoryId is required")]
    public int? SubCategoryId { get; set; }

    /// <summary>
    /// SubCategoryGuid
    /// </summary>
    [Required(ErrorMessage = "SubCategoryGuid is required")]
    public Guid? SubCategoryGuid { get; set; }

    /// <summary>
    /// Name of Entry
    /// </summary>
    [Required(ErrorMessage = $"{nameof(Name)} is required")]
    [MaxLength(100, ErrorMessage = $"Max length for is {nameof(Name)} 100.")]
    public string Name { get; set; }

    /// <summary>
    /// Description of Entry
    /// </summary>
    [Required(ErrorMessage = $"{nameof(Description)} is required")]
    [MaxLength(500, ErrorMessage = $"Max length for is {nameof(Description)} 500.")]
    public string Description { get; set; }

    /// <summary>
    /// Url for Entry
    /// </summary>
    [MaxLength(100, ErrorMessage = $"Max length for is {nameof(Url)} 100.")]
    public string Url { get; set; }

    /// <summary>
    /// Image for Entry
    /// </summary>
    [MaxLength(100, ErrorMessage = $"Max length for is {nameof(Image)} 100.")]
    public string Image { get; set; }

    /// <summary>
    /// Active Flag
    /// </summary>
    public bool Active { get; set; }
}
