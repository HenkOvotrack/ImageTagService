using Ovotrack4.ImageTag.Service.Models;

namespace Ovotrack4.ImageTag.Service.Interfaces
{
    public interface IImageTagsService
    {
        Task<IEnumerable<ImageTagDto>> GetItemsAsync();
        Task<Dictionary<string, string>> GetItemsFormattedAsync();
        Task<ImageTagDto?> GetItemAsync(string id);
        Task<bool> AddItemAsync(ImageTagDto item);
        Task<bool> AddPushMessageAsync(ImageTagPushMessage item);
        Task<ImageTagDto?> UpdateItemAsync(string id, ImageTagDto item);
        Task <bool>DeleteItemAsync(string id);
    }
}
