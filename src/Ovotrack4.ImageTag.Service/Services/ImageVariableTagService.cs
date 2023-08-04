using Ovotrack4.ImageTag.Service.Interfaces;
using Ovotrack4.ImageTag.Service.Models;

namespace Ovotrack4.ImageTag.Service.Services
{
    public class ImageVariableTagService:IImageVariableTagService
    {
        private readonly ILogger<ImageVariableTagService> _logger;
        private readonly IImageTagsService _imageTagsService;
        private readonly IImageVariableService _imageVariablesService;

        public ImageVariableTagService(ILogger<ImageVariableTagService> logger, IImageTagsService imageTagsService, IImageVariableService imageVariableService)
        {
            _logger = logger;
            _imageTagsService = imageTagsService;
            _imageVariablesService = imageVariableService;
            
        }

        public async Task<IEnumerable<ImageVariableTagDto>> GetItemsAsync()
        {
            var imageVarTags = new List<ImageVariableTagDto>();
            if (imageVarTags == null) throw new ArgumentNullException(nameof(imageVarTags));
            var imageTags = await _imageTagsService.GetItemsAsync();
            var imageVariables = await _imageVariablesService.GetItemsAsync();
            imageVarTags.AddRange(from imageTag in imageTags let imageVariableMappingDtos = imageVariables.ToList() let imageVariable = imageVariableMappingDtos.FirstOrDefault(x => x.imageName == imageTag.repository) where imageVariable != null select new ImageVariableTagDto { imageName = imageTag.repository, variableName = imageVariable.variableName, latestVersionTag = imageTag.tag, });
            return imageVarTags;
        }


        public async Task<Dictionary<string, string>> GetItemsFormattedAsync()
        {
            var imageVarTags = new Dictionary<string, string>();
            var tmpItems = GetItemsAsync();
            foreach (var item in tmpItems.Result)
            {
                if (item.latestVersionTag != null) imageVarTags.Add(item.variableName, item.latestVersionTag);
            }
            return await Task.FromResult(imageVarTags);
        }
    }
}
