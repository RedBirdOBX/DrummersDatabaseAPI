using DrummersDatabaseAPI.Dtos;

namespace DrummersDatabaseAPI.Service
{
    public interface ICategoryProcessor
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync(bool showAll);

        Task<CategoryDto?> GetCategoryAsync(int categoryId, bool includeSubCategories);

        Task<CategoryWithoutSubCategoriesDto> CreateCategoryAsync(CategoryCreateDto category);

        Task<CategoryWithoutSubCategoriesDto> UpdateCategoryAsync(int categoryId, CategoryUpdateDto category);

        Task<bool> PatchCategoryAsync(int categoryId, CategoryUpdateDto input);

        Task<bool> DoesCategoryExistAsync(int categoryId);
    }
}
