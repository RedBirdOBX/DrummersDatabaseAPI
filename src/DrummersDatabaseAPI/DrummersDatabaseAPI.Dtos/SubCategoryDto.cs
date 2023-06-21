namespace DrummersDatabaseAPI.Dtos;

/// <summary>
///
/// </summary>
public class SubCategoryDto : LinkedResourcesDto
{
    /// <summary>
    /// default constructor
    /// </summary>
    public SubCategoryDto()
    {
        Name = string.Empty;
        Image = string.Empty;
        Entries = new List<EntryDto>();
    }

    /// <summary>
    /// SubCategory Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// SubCategory Guid
    /// </summary>
    public Guid SubCategoryGuid { get; set; }

    /// <summary>
    /// CategoryId
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Category Guid
    /// </summary>
    public Guid CategoryGuid { get; set; }

    /// <summary>
    /// Name of SubCategory
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Image of SubCategory
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

    /// <summary>
    /// Entries for SubCategory
    /// </summary>
    public List<EntryDto> Entries { get; set; }

    /// <summary>
    /// Number of Entries for SubCategory
    /// </summary>
    public int TotalEntries => Entries.Count;
}
