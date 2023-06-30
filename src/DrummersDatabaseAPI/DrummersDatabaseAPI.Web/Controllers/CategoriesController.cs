using AutoMapper;
using DrummersDatabaseAPI.Dtos;
using DrummersDatabaseAPI.Service;
using DrummersDatabaseAPI.Web.Controllers.ResponseHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DrummersDatabaseAPI.Web.Controllers
{
    /// <summary>
    /// CategoriesController
    /// </summary>
    [Route("api/categories")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CategoriesController : ControllerBase
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryProcessor _processor;
        private readonly IMapper _mapper;

        /// <summary>
        /// CategoriesController constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="processor"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CategoriesController(ICategoryProcessor processor, IMapper mapper, ILogger<CategoriesController> logger)
        {
            _processor = processor ?? throw new ArgumentNullException(nameof(ICategoryProcessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(Mapper));
            _logger = logger;
        }

        /// <summary>
        /// lists all categories
        /// </summary>
        /// <returns>collection of CategoryWithoutSubCategoriesDto</returns>
        /// <example>{baseUrl}/api/categories</example>
        /// <param name="showAll">flag to show both inactive and active</param>
        /// <response code="200">returns collection of categories</response>
        [HttpGet("", Name = "GetCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(bool showAll = false)
        {
            try
            {
                _logger.LogInformation("Getting categories");

                var categoriesDtos = await _processor.GetCategoriesAsync(showAll);
                foreach (var categoryDto in categoriesDtos)
                {
                    categoryDto.Links.Add(UriLinkHelper.CreateLinkForCategoryWithinCollection(HttpContext.Request, categoryDto));
                }
                return Ok(categoriesDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetCategories)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }

        /// <summary>
        /// returns single category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="includeSubCategories"></param>
        /// <returns>CategoryDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}?includeSubCategories={bool}</example>
        /// <response code="200">returns requested category</response>
        [HttpGet("{categoryId}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> GetCategory(int categoryId, bool includeSubCategories = true)
        {
            try
            {
                if (!await _processor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                var categoryDto = await _processor.GetCategoryAsync(categoryId, includeSubCategories);
                categoryDto = UriLinkHelper.CreateLinksForCategory(HttpContext.Request, categoryDto);
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetCategory)}", ex);
                return StatusCode(500, $"An application error occurred. {ex}");
            }
        }

        /// <summary>
        /// creates new category
        /// </summary>
        /// <param name="input"></param>
        /// <returns>newly created CategoryDto</returns>
        /// <example>{baseUrl}/api/categories</example>
        /// <response code="201">category created</response>
        [HttpPost("", Name = "CreateCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryCreateDto input)
        {
            try
            {
                var finalCategoryDto = await _processor.CreateCategoryAsync(input);
                var results = CreatedAtRoute("GetCategory", new { categoryId = finalCategoryDto.Id }, finalCategoryDto);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CreateCategory)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }

        /// <summary>
        /// updates category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="input"></param>
        /// <returns>updated CategoryDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}</example>
        /// <response code="200">category updated</response>
        [HttpPut("{categoryId}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int categoryId, [FromBody] CategoryUpdateDto input)
        {
            try
            {
                if (!await _processor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                var updatedCategory = await _processor.UpdateCategoryAsync(categoryId, input);
                var results = Ok(updatedCategory);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateCategory)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }

        /// <summary>
        /// Allows for patch updates on category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="patchDocument"></param>
        /// <returns>CategoryUpdateDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}</example>
        /// <response code="200"> category patched</response>
        [HttpPatch("{categoryId}", Name = "PatchCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> PatchCategory(int categoryId, [FromBody] JsonPatchDocument<CategoryUpdateDto> patchDocument)
        {
            try
            {
                if (!await _processor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                // build the obj to send to _dbContext
                var category = await _processor.GetCategoryAsync(categoryId, false);
                var categoryToPatch = _mapper.Map<CategoryUpdateDto>(category);

                // applies updates
                patchDocument.ApplyTo(categoryToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // manually trigger the validation
                if (!TryValidateModel(categoryToPatch))
                {
                    return BadRequest(ModelState);
                }

                bool success = await _processor.PatchCategoryAsync(categoryId, categoryToPatch);
                if (!success)
                {
                    return StatusCode(500, "An application error occurred while updating the entry.");
                }

                var results = Ok(categoryToPatch);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateCategory)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }
    }
}
