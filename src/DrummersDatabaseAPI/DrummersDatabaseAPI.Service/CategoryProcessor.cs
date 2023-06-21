using AutoMapper;
using DrummersDatabaseAPI.Data.DbContexts;
using DrummersDatabaseAPI.Data.Entities;
using DrummersDatabaseAPI.Data.Repositories;
using DrummersDatabaseAPI.Dtos;
using Microsoft.Extensions.Logging;

namespace DrummersDatabaseAPI.Service
{
    public class CategoryProcessor : ICategoryProcessor
    {
        private IDrummersDatabaseRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryProcessor> _logger;

        public CategoryProcessor(IDrummersDatabaseRepository repository, IMapper mapper, ILogger<CategoryProcessor> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(DrummersDatabaseDbContext));
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(bool showAll)
        {
            try
            {
                var categories = await _repository.GetCategoriesAsync(showAll);
                var results = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetCategoriesAsync)}", ex);
                throw;
            }
        }

        public async Task<CategoryDto?> GetCategoryAsync(int categoryId, bool includeSubCategories)
        {
            try
            {
                await UpdateCounter(categoryId);
                var category = await _repository.GetCategoryAsync(categoryId, includeSubCategories);

                // moq is having issues here
                var results = _mapper.Map<CategoryDto>(category);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetCategoryAsync)}", ex);
                throw;
            }
        }

        public async Task<CategoryWithoutSubCategoriesDto> CreateCategoryAsync(CategoryCreateDto input)
        {
            try
            {
                var newCategoryEntity = _mapper.Map<Category>(input);
                await _repository.CreateCategoryAsync(newCategoryEntity);
                bool success = await _repository.SaveChangesAsync();

                if (!success)
                {
                    return null;
                }

                CategoryWithoutSubCategoriesDto finalCategoryDto = _mapper.Map<CategoryWithoutSubCategoriesDto>(newCategoryEntity);
                return finalCategoryDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CreateCategoryAsync)}", ex);
                throw;
            }
        }

        public async Task<CategoryWithoutSubCategoriesDto> UpdateCategoryAsync(int categoryId, CategoryUpdateDto input)
        {
            try
            {
                var category = await _repository.GetCategoryAsync(categoryId, false);
                _mapper.Map(input, category);
                bool success = await _repository.SaveChangesAsync();

                if (!success)
                {
                    return null;
                }

                CategoryWithoutSubCategoriesDto results = _mapper.Map<CategoryWithoutSubCategoriesDto>(category);
                return results;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateCategoryAsync)}", ex);
                throw;
            }
        }

        public async Task<bool> PatchCategoryAsync(CategoryUpdateDto input, int categoryId)
        {
            try
            {
                // map the source update dto to then entity / destination
                var category = await _repository.GetCategoryAsync(categoryId, false);
                _mapper.Map(input, category);

                bool success = await _repository.SaveChangesAsync();
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(PatchCategoryAsync)}", ex);
                throw;
            }
        }

        public async Task<bool> DoesCategoryExistAsync(int categoryId)
        {
            return await _repository.DoesCategoryExistAsync(categoryId);
        }

        // private
        private async Task UpdateCounter(int categoryId)
        {
            var category = await _repository.GetCategoryAsync(categoryId, false);
            if (category is not null)
            {
                category.Counter++;
                await _repository.SaveChangesAsync();
            }
        }
    }
}