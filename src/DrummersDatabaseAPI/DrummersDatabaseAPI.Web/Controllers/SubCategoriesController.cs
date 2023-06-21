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
    /// SubCategoriesController
    /// </summary>
    [Route("api/categories/{categoryId}/subcategories")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ILogger<SubCategoriesController> _logger;
        private readonly ICategoryProcessor _categoryProcessor;
        private readonly ISubCategoryProcessor _subCategoryProcessor;
        private readonly IMapper _mapper;

        /// <summary>
        /// SubCategoriesController constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="categoryProcessor"></param>
        /// <param name="subCategoryProcessor"></param>
        /// /// <param name="mapper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SubCategoriesController(ILogger<SubCategoriesController> logger, ICategoryProcessor categoryProcessor, ISubCategoryProcessor subCategoryProcessor, IMapper mapper)
        {
            _logger = logger;
            _categoryProcessor = categoryProcessor ?? throw new ArgumentNullException(nameof(ICategoryProcessor));
            _subCategoryProcessor = subCategoryProcessor ?? throw new ArgumentNullException(nameof(ISubCategoryProcessor));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(Mapper));
        }


        /// <summary>
        /// gets SubCategories
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="showAll"></param>
        /// <returns>collection of SubCategoryDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}/subcategories</example>
        /// <response code="200">collection of sub categories</response>
        [HttpGet("", Name = "GetSubCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SubCategoryDto>>> GetSubCategories(int categoryId, bool showAll = false)
        {
            try
            {
                if (!await _categoryProcessor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                var subCategoryDtos = await _subCategoryProcessor.GetSubCategoriesAsync(categoryId, showAll);
                foreach (var subCategory in subCategoryDtos)
                {
                    subCategory.Links.Add(UriLinkHelper.CreateLinkForSubCategoryWithinCollection(HttpContext.Request, subCategory));
                }
                return Ok(subCategoryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetSubCategories)}", ex);
                return StatusCode(500, $"An application error occurred. {ex}");
            }
        }

        /// <summary>
        /// Gets single SubCategory
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="includeEntries"></param>
        /// <returns>single SubCategoryDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}/subcategories/{subCategoryId}?includeEntries={bool}</example>
        /// <response code="200">sub categories</response>
        /// <response code="404">category / subcategory not found</response>
        [HttpGet("{subCategoryId}", Name = "GetSubCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SubCategoryDto>> GetSubCategory(int categoryId, int subCategoryId, bool includeEntries = true)
        {
            try
            {
                if (!await _categoryProcessor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                if (!await _subCategoryProcessor.DoesSubCategoryExistAsync(subCategoryId))
                {
                    return NotFound($"subcategory {subCategoryId} not found.");
                }

                var subCategoryDto = await _subCategoryProcessor.GetSubCategoryAsync(subCategoryId, includeEntries);
                subCategoryDto = UriLinkHelper.CreateLinksForSubCategory(HttpContext.Request, subCategoryDto);
                return Ok(subCategoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetSubCategories)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }

        /// <summary>
        /// Creates SubCategory
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="input"></param>
        /// <returns>newly created SubCategoryDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}/subcategories</example>
        /// <response code="201">sub categories created</response>
        /// <response code="404">category / subcategory not found</response>
        [HttpPost("", Name = "CreateSubCategory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<SubCategoryDto>> CreateSubCategory(int categoryId, [FromBody] SubCategoryCreateDto input)
        {
            try
            {
                if (!await _categoryProcessor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                var category = await _categoryProcessor.GetCategoryAsync(categoryId, false);
                if (input.CategoryId != category?.Id)
                {
                    return BadRequest($"category id mismatch.");
                }

                var newSubCategoryDto = await _subCategoryProcessor.CreateSubCategoryAsync(input, category);
                var results = CreatedAtRoute("GetSubCategory", new { categoryId = newSubCategoryDto.CategoryId, subCategoryId = newSubCategoryDto.Id }, newSubCategoryDto);
                return results;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CreateSubCategory)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }

        /// <summary>
        /// Updates SubCategory
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="input"></param>
        /// <returns>updated SubCategoryDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}/subcategories/{subCategoryId}</example>
        /// <response code="200">sub categories created</response>
        /// <response code="404">category / subcategory not found</response>
        [HttpPut("{subCategoryId}", Name = "UpdateSubCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SubCategoryDto>> UpdateSubCategory(int categoryId, int subCategoryId, [FromBody] SubCategoryUpdateDto input)
        {
            try
            {
                if (!await _categoryProcessor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                if (!await _subCategoryProcessor.DoesSubCategoryExistAsync(subCategoryId))
                {
                    return NotFound($"subcategory {subCategoryId} not found.");
                }

                var category = await _categoryProcessor.GetCategoryAsync(categoryId, false);
                if (input.CategoryId != category?.Id)
                {
                    return BadRequest($"category id mismatch.");
                }

                var updatedSubCategory = await _subCategoryProcessor.UpdateSubCategoryAsync(subCategoryId, input);
                var results = Ok(updatedSubCategory);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateSubCategory)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }

        /// <summary>
        /// Allows for patch updates on subcategory
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="patchDocument"></param>
        /// <returns>SubCategoryUpdateDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}/subcategories/{subCategoryId}</example>
        /// <response code="200">sub categories created</response>
        /// <response code="404">category / subcategory not found</response>
        [HttpPatch("{subCategoryId}", Name = "PatchSubCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SubCategoryDto>> PatchSubCategory(int categoryId, int subCategoryId, [FromBody] JsonPatchDocument<SubCategoryUpdateDto> patchDocument)
        {
            try
            {
                if (!await _categoryProcessor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                if (!await _subCategoryProcessor.DoesSubCategoryExistAsync(subCategoryId))
                {
                    return NotFound($"subcategory {subCategoryId} not found.");
                }

                var subCategory = await _subCategoryProcessor.GetSubCategoryAsync(subCategoryId, false);
                if (subCategoryId != subCategory?.Id)
                {
                    return BadRequest($"category id mismatch.");
                }

                // build the obj to send to _dbContext
                var subCategoryToPatch = _mapper.Map<SubCategoryUpdateDto>(subCategory);

                // applies updates
                patchDocument.ApplyTo(subCategoryToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // manually trigger the validation
                if (!TryValidateModel(subCategoryToPatch))
                {
                    return BadRequest(ModelState);
                }

                bool success = await _subCategoryProcessor.PatchSubCategoryAsync(subCategoryToPatch, subCategoryId);
                if (!success)
                {
                    return StatusCode(500, "An application error occurred while updating the entry.");
                }

                var results = Ok(subCategoryToPatch);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(PatchSubCategory)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }
    }
}
