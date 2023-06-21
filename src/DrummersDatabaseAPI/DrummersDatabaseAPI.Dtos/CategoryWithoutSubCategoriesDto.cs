namespace DrummersDatabaseAPI.Dtos;

/// <summary>
/// Category Without SubCategories
/// </summary>
public class CategoryWithoutSubCategoriesDto : LinkedResourcesDto
{
    /// <summary>
    /// default constructor
    /// </summary>
    public CategoryWithoutSubCategoriesDto()
    {
        Name = string.Empty;
        Image = string.Empty;
    }

    /// <summary>
    /// Id of Category
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Guid of Category
    /// </summary>
    public Guid CategoryGuid { get; set; }

    /// <summary>
    /// Name of Category
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///  Img used for Category
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