using AutoMapper;
using DrummersDatabaseAPI.Data.DbContexts;
using DrummersDatabaseAPI.Data.Entities;
using DrummersDatabaseAPI.Data.Repositories;
using DrummersDatabaseAPI.Dtos;
using Microsoft.Extensions.Logging;

namespace DrummersDatabaseAPI.Service
{
    public class EntryProcessor : IEntryProcessor
    {
        private IDrummersDatabaseRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EntryProcessor> _logger;

        public EntryProcessor(IDrummersDatabaseRepository repository, IMapper mapper, ILogger<EntryProcessor> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(DrummersDatabaseDbContext));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EntryDto>> GetEntriesAsync(int subCategoryId, string? filter, string? search, int pageNum, int pageSize, bool showAll)
        {
            try
            {
                var entries = await _repository.GetEntriesAsync(subCategoryId, filter, search, pageNum, pageSize, showAll);
                var results = _mapper.Map<IEnumerable<EntryDto>>(entries);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetEntriesAsync)}", ex);
                throw;
            }
        }

        public async Task<EntryDto?> GetEntryAsync(int entryId)
        {
            try
            {
                await UpdateCounter(entryId);
                var entry = await _repository.GetEntryAsync(entryId);
                var results = _mapper.Map<EntryDto>(entry);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetEntryAsync)}", ex);
                throw;
            }
        }

        public async Task<int> GetCountOfEntriesAsync()
        {
            try
            {
                var count = await _repository.GetCountOfEntriesAsync();
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetCountOfEntriesAsync)}", ex);
                throw;
            }
        }

        public async Task<EntryDto> CreateEntryAsync(EntryCreateDto input, SubCategoryDto subCategory)
        {
            try
            {
                var newEntryEntity = _mapper.Map<Entry>(input);
                newEntryEntity.SubCategoryId = subCategory.Id;
                newEntryEntity.SubCategoryGuid = subCategory.SubCategoryGuid;

                await _repository.CreateEntryAsync(newEntryEntity);
                bool success = await _repository.SaveChangesAsync();

                if (!success)
                {
                    return null;
                }

                EntryDto newEntryDto = _mapper.Map<EntryDto>(newEntryEntity);
                return newEntryDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CreateEntryAsync)}", ex);
                throw;
            }
        }

        public async Task<EntryDto> UpdateEntryAsync(EntryUpdateDto input, int entryId)
        {
            try
            {
                var entry = await _repository.GetEntryAsync(entryId);
                _mapper.Map(input, entry);
                bool success = await _repository.SaveChangesAsync();

                if (!success)
                {
                    return null;
                }

                EntryDto results = _mapper.Map<EntryDto>(entry);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateEntryAsync)}", ex);
                throw;
            }
        }

        public async Task<bool> PatchEntryAsync(EntryUpdateDto input, int entryId)
        {
            try
            {
                // map the source update dto to then entity / destination
                var entry = await _repository.GetEntryAsync(entryId);
                _mapper.Map(input, entry);

                bool success = await _repository.SaveChangesAsync();
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(PatchEntryAsync)}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteEntryAsync(int entryId)
        {
            try
            {
                var entry = await _repository.GetEntryAsync(entryId);
                 _repository.DeleteEntry(entry);

                // just for development / demo
                //bool success = await _repository.SaveChangesAsync();
                //return success;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(DeleteEntryAsync)}", ex);
                throw;
            }
        }

        public async Task<bool> DoesEntryExistAsync(int entryId)
        {
            return await _repository.DoesEntryExistAsync(entryId);
        }

        // private
        private async Task UpdateCounter(int entryId)
        {
            var entry = await _repository.GetEntryAsync(entryId);
            if (entry is not null)
            {
                entry.Counter++;
                await _repository.SaveChangesAsync();
            }
        }
    }
}