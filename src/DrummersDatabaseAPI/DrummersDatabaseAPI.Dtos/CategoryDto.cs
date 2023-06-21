namespace DrummersDatabaseAPI.Dtos;


/// <summary>
/// Category with SubCategories
/// </summary>
public class CategoryDto : LinkedResourcesDto
{
    /// <summary>
    /// constructor
    /// </summary>
    public CategoryDto()
    {
        SubCategories = new List<SubCategoryDto>();
        Name = string.Empty;
        Image = string.Empty;
    }

    /// <summary>
    /// category id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// category guid
    /// </summary>
    public Guid CategoryGuid { get; set; }

    /// <summary>
    /// category name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// image for category
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
    /// list of sub categories
    /// </summary>
    public ICollection<SubCategoryDto> SubCategories { get; set; }

    /// <summary>
    /// count of sub categories
    /// </summary>
    public int TotalSubCategories => SubCategories.Count;
}