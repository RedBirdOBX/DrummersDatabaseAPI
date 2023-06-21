using DrummersDatabaseAPI.Data.Entities;

namespace DrummersDatabaseAPI.Data.Repositories
{
    public interface IDrummersDatabaseRepository
    {
        // categories
        Task<IEnumerable<Category>> GetCategoriesAsync(bool showAll);

        Task<Category?> GetCategoryAsync(int categoryId, bool includeSubCategories);

        Task CreateCategoryAsync(Category category);

        Task<bool> DoesCategoryExistAsync(int categoryId);

        // subcategories
        Task<IEnumerable<SubCategory>> GetSubCategoriesAsync(int categoryId, bool showAll);

        Task<SubCategory?> GetSubCategoryAsync(int subCategoryId, bool includeEntries);

        Task CreateSubCategoryAsync(SubCategory subCategory);

        Task<bool> DoesSubCategoryExistAsync(int subCategoryId);

        // entries
        Task<IEnumerable<Entry>> GetEntriesAsync(int subCategoryId, string? filter, string? search, int pageNum, int pageSize, bool showAll);

        Task<Entry?> GetEntryAsync(int entryId);

        Task<int> GetCountOfEntriesAsync();

        Task CreateEntryAsync(Entry entry);

        void DeleteEntry(Entry entry);

        Task<bool> DoesEntryExistAsync(int entryId);

        // global
        Task<bool> SaveChangesAsync();
    }
}
