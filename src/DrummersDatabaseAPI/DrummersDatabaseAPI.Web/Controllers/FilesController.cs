using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace DrummersDatabaseAPI.Web.Controllers
{
    /// <summary>
    /// Files controller
    /// </summary>
    [Route("api/files")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        private readonly ILogger<FilesController> _logger;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="fileExtensionContentTypeProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider, ILogger<FilesController> logger)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? throw new ArgumentNullException(nameof(fileExtensionContentTypeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// streams requested file if found
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>File</returns>
        [HttpGet("{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetFile(string fileId)
        {
            try
            {
                // choices..
                // FileContentResult
                // FileStreamResult
                // PhysicalFileResult
                // VirtualFileResult

                string file = string.Empty;
                switch (fileId)
                {
                    case "1":
                        file = "Downloads\\Test.txt";
                        break;
                    case "2":
                        file = "Downloads\\paper-bill.pdf";
                        break;
                    default:
                        file = "Downloads\\Test.txt";
                        break;
                }

                if (!System.IO.File.Exists(file))
                {
                    return NotFound($"file {file} not found.");
                }

                if (!_fileExtensionContentTypeProvider.TryGetContentType(file, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                var bytes = System.IO.File.ReadAllBytes(file);
                return File(bytes, contentType, Path.GetFileName(file));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetFile: {ex}");
                throw;
            }
        }
    }
}
