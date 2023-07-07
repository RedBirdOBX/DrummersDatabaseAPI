using System.ComponentModel.DataAnnotations;

namespace DrummersDatabaseAPI.Dtos;

/// <summary>
/// Dto for creating Entry
/// </summary>
public class EntryCreateDto
{
    /// <summary>
    /// default constructor
    /// </summary>
    public EntryCreateDto()
    {
        Name = string.Empty;
        Description = string.Empty;
        Url = string.Empty;
        Image = string.Empty;
        Active = true;
        EntryGuid = Guid.NewGuid();
    }

    /// <summary>
    /// Entry Guid
    /// </summary>
    public Guid EntryGuid { get; set; }

    /// <summary>
    /// SubCategoryId
    /// </summary>
    [Required(ErrorMessage = "SubCategoryId is required")]
    public int? SubCategoryId { get; set; }

    /// <summary>
    /// SubCategory Guid
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
    /// Image of Entry
    /// </summary>
    [MaxLength(100, ErrorMessage = $"Max length for is {nameof(Image)} 100.")]
    public string Image { get; set; }

    /// <summary>
    /// Active Flag
    /// </summary>
    public bool Active { get; set; }
}
