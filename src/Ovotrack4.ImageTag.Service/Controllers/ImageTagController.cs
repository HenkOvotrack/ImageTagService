using Microsoft.AspNetCore.Mvc;
using Ovotrack4.ImageTag.Service.Interfaces;
using Ovotrack4.ImageTag.Service.Models;

namespace Ovotrack4.ImageTag.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageTagController : ControllerBase
    {
        private readonly ILogger<ImageTagController> _logger;
        private readonly IImageTagsService _imageTagsService;

        // Initialize controller
        // Inject ILogger
        // Inject IImageTagsService
        public ImageTagController(ILogger<ImageTagController> logger,IImageTagsService imageTagsService)
        {
            _logger = logger;
           _imageTagsService = imageTagsService;
        }

       
        // post endpoint for json object
        // deserialize json object to ImageTageDto
        // check if image tag already exists
        // if not, create new image tag
        // return 201 Created response and the image tag
        // return 400 Bad Request if the image tag is invalid
        // return 409 Conflict if the image tag already exists
        // return 500 Internal Server Error if something goes wrong
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ImageTagPushMessage>> PostImageTag(ImageTagPushMessage imageTag)
        {
            _logger.LogInformation("PostImageTag called");
            _logger.LogInformation($"ImageTag: {imageTag}");

            if (!await _imageTagsService.AddPushMessageAsync(imageTag))
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetImageTag), new { id = imageTag.Id }, imageTag);    
        }
        // get endpoint for image tag id
        // check if image tag exists
        // if not, return 404 Not Found
        // if yes, return 200 OK response and the image tag
        // return 500 Internal Server Error if something goes wrong
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ImageTagDto>> GetImageTag(string id)
        {
           var result = await _imageTagsService.GetItemAsync(id);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);  

        }
        // get endpoint for all image tags
        // return 200 OK response and all image tags
        // return 500 Internal Server Error if something goes wrong
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ImageTagDto>>> GetImageTags()
        {
            var results = await _imageTagsService.GetItemsAsync();
            return Ok(results);
                
           
        }

        // get endpoint for formatted image tags

        // return 200 OK response and all image tags
        // return 500 Internal Server Error if something goes wrong
        [HttpGet("formatted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Dictionary<string, string>>> GetFormattedImageTags()
        {
           

            return Ok(await _imageTagsService.GetItemsFormattedAsync());
        }

        // delete endpoint for image tag id
        // check if image tag exists
        // if not, return 404 Not Found
        // if yes, delete image tag
        // return 204 No Content response
        // return 500 Internal Server Error if something goes wrong
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImageTag(string id)
        {
            var retVal = await _imageTagsService.DeleteItemAsync(id);
            if (!retVal)
            {
                return NotFound();
            }
            return NoContent();
           
        }

        

    }
}
