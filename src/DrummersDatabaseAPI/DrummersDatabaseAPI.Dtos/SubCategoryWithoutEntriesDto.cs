namespace DrummersDatabaseAPI.Dtos;

/// <summary>
/// SubCategoryWithoutEntriesDto
/// </summary>
public class SubCategoryWithoutEntriesDto
{
    /// <summary>
    /// default constructor
    /// </summary>
    public SubCategoryWithoutEntriesDto()
    {
        Name = string.Empty;
        Image = string.Empty;
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
    /// Category Id
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Category Guid
    /// </summary>
    public Guid CategoryGuid { get; set; }

    /// <summary>
    /// SubCategory Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Image of SubCategory
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Counter of times clicked
    /// </summary>
    public int Counter { get; set; }

    /// <summary>
    /// Active Flag
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Created Date
    /// </summary>
    public DateTime Created { get; set; }
}
