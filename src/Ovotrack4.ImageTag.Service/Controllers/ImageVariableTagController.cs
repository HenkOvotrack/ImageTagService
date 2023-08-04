using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ovotrack4.ImageTag.Service.Interfaces;
using Ovotrack4.ImageTag.Service.Models;
using Ovotrack4.ImageTag.Service.Services;

namespace Ovotrack4.ImageTag.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageVariableTagController : ControllerBase
    {
        private readonly ILogger<ImageVariableTagController> _logger;
        private readonly IImageVariableTagService _imageVariableTagService;
        public ImageVariableTagController(ILogger<ImageVariableTagController> logger, IImageVariableTagService imageVariableTagService)
        {
            _logger = logger;
            _imageVariableTagService = imageVariableTagService;
        }

        // get endpoint for all image tags
        // return 200 OK response and all image tags
        // return 500 Internal Server Error if something goes wrong
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ImageVariableTagDto>>> GetImageVariableTags()
        {
            var results = await _imageVariableTagService.GetItemsAsync();
            return Ok(results);


        }

        // get endpoint for formatted image tags

        // return 200 OK response and all image tags
        // return 500 Internal Server Error if something goes wrong
        [HttpGet("formatted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Dictionary<string, string>>> GetFormattedImageVariableTags()
        {


            return Ok(await _imageVariableTagService.GetItemsFormattedAsync());
        }
    }
}
