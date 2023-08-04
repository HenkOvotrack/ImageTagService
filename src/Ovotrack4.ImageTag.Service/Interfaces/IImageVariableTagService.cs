using Ovotrack4.ImageTag.Service.Models;

namespace Ovotrack4.ImageTag.Service.Interfaces
{
    public interface IImageVariableTagService
    {
        Task<IEnumerable<ImageVariableTagDto>> GetItemsAsync();
        Task<Dictionary<string, string>> GetItemsFormattedAsync();
    }
}
