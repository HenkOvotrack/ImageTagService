using Ovotrack4.ImageTag.Service.Models;

namespace Ovotrack4.ImageTag.Service.Interfaces
{
    public interface IImageVariableService
    {
        Task<IEnumerable<ImageVariableMappingDto>> GetItemsAsync();
        Task<Dictionary<string, string?>> GetItemsFormattedAsync();
        Task<ImageVariableMappingDto?> GetItemAsync(string id);
        Task <bool>AddItemAsync(ImageVariableMappingDto item);
        Task<ImageVariableMappingDto?> UpdateItemAsync(string id, ImageVariableMappingDto? item);
        Task <bool>DeleteItemAsync(string id);
    }
}
