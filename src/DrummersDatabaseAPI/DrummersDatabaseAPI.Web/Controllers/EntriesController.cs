using AutoMapper;
using DrummersDatabaseAPI.Dtos;
using DrummersDatabaseAPI.Service;
using DrummersDatabaseAPI.Web.Controllers.RequestHelpers;
using DrummersDatabaseAPI.Web.Controllers.ResponseHelpers;
using DrummersDatabaseAPI.Web.ResponseHeaders;
using DrummersDatabaseAPI.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DrummersDatabaseAPI.Web.Controllers
{
    /// <summary>
    /// EntriesController
    /// </summary>
    [Route("api/categories/{categoryId}/subcategories/{subCategoryId}/entries")]
    [ApiController]
    [Authorize(Policy = "EntriesRequireClaim")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class EntriesController : ControllerBase
    {
        private readonly ILogger<EntriesController> _logger;
        private readonly IMailService _mailService;
        private readonly ICategoryProcessor _categoryProcessor;
        private readonly ISubCategoryProcessor _subCategoryProcessor;
        private readonly IEntryProcessor _entryProcessor;
        private readonly IMapper _mapper;

        /// <summary>
        /// EntriesController constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mailService"></param>
        /// <param name="categoryProcessor"></param>
        /// <param name="subCategoryProcessor"></param>
        /// <param name="entryProcessor"></param>
        /// <param name="mapper"></param>
        /// /// <exception cref="ArgumentNullException"></exception>
        public EntriesController(ILogger<EntriesController> logger, IMailService mailService, ICategoryProcessor categoryProcessor,
            ISubCategoryProcessor subCategoryProcessor, IEntryProcessor entryProcessor, IMapper mapper)
        {
            _logger = logger;
            _mailService = mailService;
            _categoryProcessor = categoryProcessor;
            _subCategoryProcessor = subCategoryProcessor;
            _entryProcessor = entryProcessor;
            _mapper = mapper;
        }

        /// <summary>
        /// returns collection of EntryDtos
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="filter"></param>
        /// <returns>collection of EntryDtos</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}/subcategories/{subCategoryId}/entries</example>
        /// <response code="200">collection of entries</response>
        [HttpGet("", Name = "GetEntries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EntryDto>>> GetEntries(int categoryId, int subCategoryId, [FromQuery] string? filter, string? search, int page = 1, int size = 50, bool showAll = false)
        {
            try
            {
                if (!await _categoryProcessor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                var category = await _categoryProcessor.GetCategoryAsync(categoryId, false);
                if (categoryId != category?.Id)
                {
                    return BadRequest($"category id mismatch.");
                }

                if (!await _subCategoryProcessor.DoesSubCategoryExistAsync(subCategoryId))
                {
                    return NotFound($"subcategory {subCategoryId} not found.");
                }

                var allCount = await _entryProcessor.GetCountOfEntriesAsync();
                int pageSize = DefaultRequestParameters.GetPageSize(size);
                int pageNum = DefaultRequestParameters.GetPageNumber(page, allCount, pageSize);

                var entries = await _entryProcessor.GetEntriesAsync(subCategoryId, filter, search, pageNum, pageSize, showAll);

                foreach (var entry in entries)
                {
                    entry.Links.Add(UriLinkHelper.CreateLinkForEntriesWithinCollection(HttpContext.Request, entry, categoryId));
                }

                var headerObj = new PaginationMetaData(allCount, pageSize, pageNum);
                Response.Headers.Add("x-pagination", JsonSerializer.Serialize(headerObj));

                return Ok(entries);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetEntries)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }

        /// <summary>
        /// returns EntryDto
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="entryId"></param>
        /// <returns>EntryDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}/subcategories/{subCategoryId}/entries/{entryId}</example>
        /// <response code="200">entry</response>
        [HttpGet("{entryId}", Name = "GetEntry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<EntryDto>> GetEntry(int categoryId, int subCategoryId, int entryId)
        {
            try
            {
                if (!await _categoryProcessor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                var category = await _categoryProcessor.GetCategoryAsync(categoryId, false);
                if (categoryId != category?.Id)
                {
                    return BadRequest($"category id mismatch.");
                }

                if (!await _subCategoryProcessor.DoesSubCategoryExistAsync(subCategoryId))
                {
                    return NotFound($"subcategory {subCategoryId} not found.");
                }

                if (!await _entryProcessor.DoesEntryExistAsync(entryId))
                {
                    return NotFound($"entry {entryId} not found.");
                }

                var entry = await _entryProcessor.GetEntryAsync(entryId);
                entry = UriLinkHelper.CreateLinksForEntry(HttpContext.Request, entry, categoryId);
                return Ok(entry);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetEntry)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }

        /// <summary>
        /// creates new EntryDto
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="input"></param>
        /// <returns>newly created EntryDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}/subcategories/{subCategoryId}/entries</example>
        /// <response code="201">created entry</response>
        [HttpPost("", Name = "CreateEntry")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<EntryDto>> CreateEntry(int categoryId, int subCategoryId, [FromBody] EntryCreateDto input)
        {
            try
            {
                if (!await _categoryProcessor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                var category = await _categoryProcessor.GetCategoryAsync(categoryId, false);
                if (categoryId != category?.Id)
                {
                    return BadRequest($"category id mismatch.");
                }

                if (!await _subCategoryProcessor.DoesSubCategoryExistAsync(subCategoryId))
                {
                    return NotFound($"subcategory {subCategoryId} not found.");
                }

                var subCategory = await _subCategoryProcessor.GetSubCategoryAsync(subCategoryId, false);
                if (subCategoryId != subCategory?.Id)
                {
                    return BadRequest($"subCategory id mismatch.");
                }

                var newEntry = await _entryProcessor.CreateEntryAsync(input, subCategory);
                var results = CreatedAtRoute("GetEntry",
                    new
                    {
                        categoryId = categoryId,
                        subCategoryId = newEntry.SubCategoryId,
                        entryId = newEntry.Id,

                    }, newEntry);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CreateEntry)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }

        /// <summary>
        /// Updates entry
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="entryId"></param>
        /// <param name="input"></param>
        /// <returns>updated EntryDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}/subcategories/{subCategoryId}/entries/{entryId}</example>
        /// <response code="200">updated entry</response>
        [HttpPut("{entryId}", Name = "UpdateEntry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<EntryDto>> UpdateEntry(int categoryId, int subCategoryId, int entryId, [FromBody] EntryUpdateDto input)
        {
            try
            {
                if (!await _categoryProcessor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                var category = await _categoryProcessor.GetCategoryAsync(categoryId, false);
                if (categoryId != category?.Id)
                {
                    return BadRequest($"category id mismatch.");
                }

                if (!await _subCategoryProcessor.DoesSubCategoryExistAsync(subCategoryId))
                {
                    return NotFound($"subcategory {subCategoryId} not found.");
                }

                var subCategory = await _subCategoryProcessor.GetSubCategoryAsync(subCategoryId, false);
                if (subCategoryId != subCategory?.Id)
                {
                    return BadRequest($"subCategory id mismatch.");
                }

                if (!await _entryProcessor.DoesEntryExistAsync(entryId))
                {
                    return NotFound($"entry {entryId} not found.");
                }

                var updatedEntry = await _entryProcessor.UpdateEntryAsync(entryId, input); ;
                var results = Ok(updatedEntry);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateEntry)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }

        /// <summary>
        /// Allows for patch updates on entry
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="entryId"></param>
        /// <param name="patchDocument"></param>
        /// <returns>EntryUpdateDto</returns>
        /// <example>{baseUrl}/api/categories/{categoryId}/subcategories/{subCategoryId}/entries/{entryId}</example>
        /// <response code="200">patched entry</response>
        [HttpPatch("{entryId}", Name = "PatchEntry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<EntryDto>> PatchEntry(int categoryId, int subCategoryId, int entryId, [FromBody] JsonPatchDocument<EntryUpdateDto> patchDocument)
        {
            try
            {
                if (!await _categoryProcessor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                var category = await _categoryProcessor.GetCategoryAsync(categoryId, false);
                if (categoryId != category?.Id)
                {
                    return BadRequest($"category id mismatch.");
                }

                if (!await _subCategoryProcessor.DoesSubCategoryExistAsync(subCategoryId))
                {
                    return NotFound($"subcategory {subCategoryId} not found.");
                }

                var subCategory = await _subCategoryProcessor.GetSubCategoryAsync(subCategoryId, false);
                if (subCategoryId != subCategory?.Id)
                {
                    return BadRequest($"subCategory id mismatch.");
                }

                var entry = await _entryProcessor.GetEntryAsync(entryId);
                if (entry is null)
                {
                    return NotFound($"entry {entryId} not found.");
                }

                // build the obj to send to _dbContext
                var entryToPatch = _mapper.Map<EntryUpdateDto>(entry);

                // applies updates
                patchDocument.ApplyTo(entryToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // manually trigger the validation
                if (!TryValidateModel(entryToPatch))
                {
                    return BadRequest(ModelState);
                }

                // map the source update dto to then entity / destination
                _mapper.Map(entryToPatch, entry);

                bool success = await _entryProcessor.PatchEntryAsync(entryId, entryToPatch);
                if (!success)
                {
                    return StatusCode(500, "An application error occurred while updating the entry.");
                }

                var results = Ok(entryToPatch);
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(PatchEntry)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }

        // demo only
        /// <summary>
        /// deletes entry
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="entryId"></param>
        /// <example>{baseUrl}/api/categories/{categoryId}/subcategories/{subCategoryId}/entries/{entryId}</example>
        /// <response code="204">patched entry</response>
        [HttpDelete("{entryId}", Name = "DeleteEntry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<EntryDto>> DeleteEntry(int categoryId, int subCategoryId, int entryId)
        {
            try
            {
                if (!await _categoryProcessor.DoesCategoryExistAsync(categoryId))
                {
                    return NotFound($"category {categoryId} not found.");
                }

                var category = await _categoryProcessor.GetCategoryAsync(categoryId, false);
                if (categoryId != category?.Id)
                {
                    return BadRequest($"category id mismatch.");
                }

                if (!await _subCategoryProcessor.DoesSubCategoryExistAsync(subCategoryId))
                {
                    return NotFound($"subcategory {subCategoryId} not found.");
                }

                var subCategory = await _subCategoryProcessor.GetSubCategoryAsync(subCategoryId, false);
                if (subCategoryId != subCategory?.Id)
                {
                    return BadRequest($"subCategory id mismatch.");
                }

                var entry = await _entryProcessor.GetEntryAsync(entryId);
                if (entry is null)
                {
                    return NotFound($"entry {entryId} not found.");
                }

                bool success = await _entryProcessor.DeleteEntryAsync(entryId);
                if (!success)
                {
                    return StatusCode(500, "An application error occurred while updating the entry.");
                }

                _logger.LogWarning($"entry {entryId} was deleted.");
                _mailService.Send("deleted entry", $"entry {entryId} was deleted.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(DeleteEntry)}", ex);
                return StatusCode(500, "An application error occurred.");
            }
        }
    }
}
