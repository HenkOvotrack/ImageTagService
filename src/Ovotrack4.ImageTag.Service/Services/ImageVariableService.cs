using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Ovotrack4.ImageTag.Service.Interfaces;
using Ovotrack4.ImageTag.Service.Models;

namespace Ovotrack4.ImageTag.Service.Services
{
    public class ImageVariableService: IImageVariableService
    {
        private readonly ILogger<ImageTagsService> _logger;
        private readonly Container _cosmosContainer;

        public ImageVariableService(ILogger<ImageTagsService> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosContainer = cosmosClient.GetContainer("OvotrackImageTags", "ImageMappings");
        }

        public async Task<IEnumerable<ImageVariableMappingDto>> GetItemsAsync()
        {
            // Get LINQ IQueryable object
            var queryable = _cosmosContainer.GetItemLinqQueryable<ImageVariableMappingDto>();
            // Construct LINQ query
            var matches = queryable
                .Where(p => p.variableName != null);
            // Convert to feed iterator
            using var linqFeed = matches.ToFeedIterator();
            // Read the next page of results
            var results = new List<ImageVariableMappingDto>();
            while (linqFeed.HasMoreResults)
            {
                results.AddRange(await linqFeed.ReadNextAsync());
            }
            return await Task.FromResult<IEnumerable<ImageVariableMappingDto>>(results);
        }

        public async Task<Dictionary<string, string?>> GetItemsFormattedAsync()
        {
            // Get LINQ IQueryable object
            var queryable = _cosmosContainer.GetItemLinqQueryable<ImageVariableMappingDto>();
            // Construct LINQ query
            var matches = queryable
                .Where(p => p.variableName != null);
            // Convert to feed iterator
            using var linqFeed = matches.ToFeedIterator();
            // Read the next page of results
            var results = new List<ImageVariableMappingDto>();
            while (linqFeed.HasMoreResults)
            {
                results.AddRange(await linqFeed.ReadNextAsync());
            }
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
            var innerDictionary = results.ToDictionary(item => item.variableName, item => item.imageName);
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            return (await Task.FromResult<Dictionary<string, string>>(result: innerDictionary))!;
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        }

        public async Task<ImageVariableMappingDto?> GetItemAsync(string id)
        {
            // Get LINQ IQueryable object
            var queryable = _cosmosContainer.GetItemLinqQueryable<ImageVariableMappingDto>();
            // Construct LINQ query
            var matches = queryable
                .Where(p => p.id == id);
            // Convert to feed iterator
            using var linqFeed = matches.ToFeedIterator();
            // Read the next page of results
            var results = new List<ImageVariableMappingDto>();
            while (linqFeed.HasMoreResults)
            {
                results.AddRange(await linqFeed.ReadNextAsync());
            }
            return await Task.FromResult(results.FirstOrDefault());
        }

        public async Task<bool> AddItemAsync(ImageVariableMappingDto item)
        {
            if ( item.variableName == null || item.imageName == null)
            {
                _logger.LogError("ImageVariableMapping is not correct");
                return await Task.FromResult(false);
            }
            item.id = Guid.NewGuid().ToString();
            var result = _cosmosContainer.CreateItemAsync(item, new PartitionKey(item.id));
            return await Task.FromResult(true);
        }

        public async Task<ImageVariableMappingDto?> UpdateItemAsync(string id, ImageVariableMappingDto? item)
        {
            if (item?.id == null || item.variableName == null || item.imageName == null)
            {
                _logger.LogError("ImageVariableMapping is not correct");
                return await Task.FromResult<ImageVariableMappingDto>(null!);
            }
            await _cosmosContainer.UpsertItemAsync(item, new PartitionKey(item.id));
            return await Task.FromResult(item);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            // Get LINQ IQueryable object
            var queryable = _cosmosContainer.GetItemLinqQueryable<ImageVariableMappingDto>();
            // Construct LINQ query
            var matches = queryable
                .Where(p => p.id == id);
            // Convert to feed iterator
            using var linqFeed = matches.ToFeedIterator();
            // Read the next page of results
            var results = new List<ImageVariableMappingDto>();
            while (linqFeed.HasMoreResults)
            {
                results.AddRange(await linqFeed.ReadNextAsync());
            }
            // Delete the item
            if (results.Count == 0)
            {
                return false;
            }
            await _cosmosContainer.DeleteItemAsync<ImageVariableMappingDto>(results[0].id, new PartitionKey(results[0].id));

            return true;
        }
    }
}
