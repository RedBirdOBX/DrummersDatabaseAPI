using DrummersDatabaseAPI.Dtos;

namespace DrummersDatabaseAPI.Service
{
    public interface IEntryProcessor
    {
        Task<IEnumerable<EntryDto>> GetEntriesAsync(int subCategoryId, string? filter, string? search, int pageNum, int pageSize, bool showAll);

        Task<EntryDto?> GetEntryAsync(int entryId);

        Task<int> GetCountOfEntriesAsync();

        Task<EntryDto> CreateEntryAsync(EntryCreateDto input, SubCategoryDto subCategory);

        Task<EntryDto> UpdateEntryAsync(EntryUpdateDto input, int entryId);

        Task<bool> PatchEntryAsync(EntryUpdateDto input, int entryId);

        Task<bool> DeleteEntryAsync(int entryId);

        Task<bool> DoesEntryExistAsync(int entryId);
    }
}
