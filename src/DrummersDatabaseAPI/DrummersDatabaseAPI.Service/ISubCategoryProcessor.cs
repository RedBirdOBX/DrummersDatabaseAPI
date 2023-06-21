using DrummersDatabaseAPI.Dtos;

namespace DrummersDatabaseAPI.Service
{
    public interface ISubCategoryProcessor
    {
        Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync(int categoryId, bool showAll);

        Task<SubCategoryDto?> GetSubCategoryAsync(int categoryId, bool includeEntries);

        Task<SubCategoryDto> CreateSubCategoryAsync(SubCategoryCreateDto input, CategoryDto category);

        Task<SubCategoryDto> UpdateSubCategoryAsync(int subCategoryId, SubCategoryUpdateDto input);

        Task<bool> PatchSubCategoryAsync(SubCategoryUpdateDto input, int subCategoryId);

        Task<bool> DoesSubCategoryExistAsync(int subCategoryId);
    }
}
