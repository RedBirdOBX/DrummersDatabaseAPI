namespace DrummersDatabaseAPI.Dtos;

/// <summary>
/// Entry Dto
/// </summary>
public class EntryDto : LinkedResourcesDto
{
    /// <summary>
    /// default constructor
    /// </summary>
    public EntryDto()
    {
        Name = string.Empty;
        Description = string.Empty;
        Url = string.Empty;
        Image = string.Empty;
        Created = DateTime.Now;
    }

    /// <summary>
    /// Id of Entry
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Guid for Entry
    /// </summary>
    public Guid EntryGuid { get; set; }

    /// <summary>
    /// SubcategoryId
    /// </summary>
    public int SubCategoryId { get; set; }

    /// <summary>
    /// SubCategory Guid
    /// </summary>
    public Guid SubCategoryGuid { get; set; }

    /// <summary>
    /// Name of Entry
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Description of Entry
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Url of Entry
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Image for Entry
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// counter of times clicked
    /// </summary>
    public int Counter { get; set; }

    /// <summary>
    /// Counter of times clicked
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// created date
    /// </summary>
    public DateTime Created { get; set; }
}
