using AutoMapper;
using DrummersDatabaseAPI.Data.DbContexts;
using DrummersDatabaseAPI.Data.Entities;
using DrummersDatabaseAPI.Data.Repositories;
using DrummersDatabaseAPI.Dtos;
using Microsoft.Extensions.Logging;

namespace DrummersDatabaseAPI.Service
{
    public class SubCategoryProcessor : ISubCategoryProcessor
    {
        private IDrummersDatabaseRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SubCategoryProcessor> _logger;

        public SubCategoryProcessor(IDrummersDatabaseRepository repository, IMapper mapper, ILogger<SubCategoryProcessor> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(DrummersDatabaseDbContext));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync(int categoryId, bool showAll)
        {
            try
            {
                var subCategories = await _repository.GetSubCategoriesAsync(categoryId, showAll);
                var results = _mapper.Map<IEnumerable<SubCategoryDto>>(subCategories);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetSubCategoriesAsync)}", ex);
                throw;
            }
        }

        public async Task<SubCategoryDto?> GetSubCategoryAsync(int categoryId, bool includeEntries = false)
        {
            try
            {
                await UpdateCounter(categoryId);
                var subCategory = await _repository.GetSubCategoryAsync(categoryId, includeEntries);
                var results = _mapper.Map<SubCategoryDto>(subCategory);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetSubCategoryAsync)}", ex);
                throw;
            }
        }

        public async Task<SubCategoryDto> CreateSubCategoryAsync(SubCategoryCreateDto input, CategoryDto category)
        {
            try
            {
                var newSubCategoryEntity = _mapper.Map<SubCategory>(input);
                newSubCategoryEntity.CategoryId = category.Id;
                newSubCategoryEntity.CategoryGuid = category.CategoryGuid;

                await _repository.CreateSubCategoryAsync(newSubCategoryEntity);
                bool success = await _repository.SaveChangesAsync();

                if (!success)
                {
                    return null;
                }

                SubCategoryDto newSubCategoryDto = _mapper.Map<SubCategoryDto>(newSubCategoryEntity);
                return newSubCategoryDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CreateSubCategoryAsync)}", ex);
                throw;
            }
        }

        public async Task<SubCategoryDto> UpdateSubCategoryAsync(int subCategoryId, SubCategoryUpdateDto input)
        {
            try
            {
                var subCategory = await _repository.GetSubCategoryAsync(subCategoryId, false);
                _mapper.Map(input, subCategory);
                bool success = await _repository.SaveChangesAsync();

                if (!success)
                {
                    return null;
                }

                SubCategoryDto results = _mapper.Map<SubCategoryDto>(subCategory);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateSubCategoryAsync)}", ex);
                throw;
            }
        }

        public async Task<bool> PatchSubCategoryAsync(SubCategoryUpdateDto input, int subCategoryId)
        {
            try
            {
                // map the source update dto to then entity / destination
                var subCategory = await _repository.GetSubCategoryAsync(subCategoryId, false);
                _mapper.Map(input, subCategory);

                bool success = await _repository.SaveChangesAsync();
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(PatchSubCategoryAsync)}", ex);
                throw;
            }
        }

        public async Task<bool> DoesSubCategoryExistAsync(int subCategoryId)
        {
            return await _repository.DoesSubCategoryExistAsync(subCategoryId);
        }

        // private
        private async Task UpdateCounter(int subCategoryId)
        {
            var subCategory = await _repository.GetSubCategoryAsync(subCategoryId, false);
            if (subCategory is not null)
            {
                subCategory.Counter++;
                await _repository.SaveChangesAsync();
            }
        }
    }
}