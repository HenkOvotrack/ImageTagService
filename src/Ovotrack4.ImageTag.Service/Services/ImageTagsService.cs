using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Ovotrack4.ImageTag.Service.Interfaces;
using Ovotrack4.ImageTag.Service.Models;
using static System.Threading.Tasks.Task;

namespace Ovotrack4.ImageTag.Service.Services
{
    public class ImageTagsService : IImageTagsService
    {
        private readonly ILogger<ImageTagsService> _logger;
        private readonly Container _cosmosContainer;

        public ImageTagsService(ILogger<ImageTagsService> logger, CosmosClient cosmosClient)
        {
            _logger = logger;
            _cosmosContainer = cosmosClient.GetContainer("OvotrackImageTags", "ImageTags");
        }
        public async Task<IEnumerable<ImageTagDto>> GetItemsAsync()
        {
            // Get LINQ IQueryable object
            var queryable = _cosmosContainer.GetItemLinqQueryable<ImageTagDto>();
            // Construct LINQ query
            var matches = queryable
                .Where(p => p.repository != null);
            // Convert to feed iterator
            using var linqFeed = matches.ToFeedIterator();
            // Read the next page of results
            var results = new List<ImageTagDto>();
            while (linqFeed.HasMoreResults)
            {
                results.AddRange(await linqFeed.ReadNextAsync());
            }
            return await FromResult<IEnumerable<ImageTagDto>>(results);
        }

        public async Task<Dictionary<string, string>> GetItemsFormattedAsync()
        {
                       // Get LINQ IQueryable object
            var queryable = _cosmosContainer.GetItemLinqQueryable<ImageTagDto>();
            // Construct LINQ query
            var matches = queryable
                .Where(p => p.repository != null);
            // Convert to feed iterator
            using var linqFeed = matches.ToFeedIterator();
            // Read the next page of results
            var results = new List<ImageTagDto>();
            while (linqFeed.HasMoreResults)
            {
                results.AddRange(await linqFeed.ReadNextAsync());
            }
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
            var innerDictionary = results.ToDictionary(containerImageTagDto => containerImageTagDto.repository, containerImageTagDto => containerImageTagDto.tag);
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
            return await FromResult<Dictionary<string, string>>(innerDictionary!);
        }
        

        public async Task<ImageTagDto?> GetItemAsync(string id)
        {
            // Get LINQ IQueryable object
            var queryable = _cosmosContainer.GetItemLinqQueryable<ImageTagDto>();
            // Construct LINQ query
            var matches = queryable
                .Where(p => p.id == id);
            // Convert to feed iterator
            using var linqFeed = matches.ToFeedIterator();
            // Read the next page of results
            var results = new List<ImageTagDto?>();
            while (linqFeed.HasMoreResults)
            {
                results.AddRange(await linqFeed.ReadNextAsync());
            }
            return await FromResult(results.FirstOrDefault());
        }

        public Task<bool> AddItemAsync(ImageTagDto item)
        {
            if (item.id == null || item.repository == null || item.tag == null)
            {
                _logger.LogError("ImageTag is not correct");
                return FromResult(false);
            }
            var result = _cosmosContainer.CreateItemAsync(item, new PartitionKey(item.id));
            return FromResult(result.IsCompletedSuccessfully);
        }

        public async Task<bool> AddPushMessageAsync(ImageTagPushMessage pushItem)
        {
            if (pushItem.Id == null || pushItem.Target?.Repository == null || pushItem.Target.Tag == null)
            {
                _logger.LogError("ImageTag is not correct");
                return await FromResult(false);
            }
            var item = new ImageTagDto
            {
                id = pushItem.Id,
                repository = pushItem.Target?.Repository,
                tag = pushItem.Target?.Tag
            };
            
            // Get LINQ IQueryable object
            var queryable = _cosmosContainer.GetItemLinqQueryable<ImageTagDto>();
            // Construct LINQ query
            var matches = queryable
                .Where(p => pushItem.Target != null && p.repository == pushItem.Target.Repository);
            // Convert to feed iterator
            using var linqFeed = matches.ToFeedIterator();

            var items = new List<ImageTagDto?>();
            
            // Iterate query result pages
            while (linqFeed.HasMoreResults)
            {
                var response = await linqFeed.ReadNextAsync();

                // Iterate query results
                items.AddRange(response);
            }
           
            var existingItem = items.FirstOrDefault();
            if (existingItem != null)
            {
                item.id = existingItem.id;
            }


            await _cosmosContainer.UpsertItemAsync(item);
            return await FromResult(true);
        }

        public async Task<ImageTagDto?> UpdateItemAsync(string id, ImageTagDto item)
        {
            if (item is { id: { }, repository: { }, tag: { } })
            {
                await _cosmosContainer.UpsertItemAsync(item, new PartitionKey(item.id));
                return await FromResult(item);
            }
            _logger.LogError("ImageTag is not correct");
            return await FromResult<ImageTagDto?>(null);
        }

        public async  Task<bool> DeleteItemAsync(string id)
        {
            // Get LINQ IQueryable object
            var queryable = _cosmosContainer.GetItemLinqQueryable<ImageTagDto>();
            // Construct LINQ query
            var matches = queryable
                .Where(p => p.id == id);
            // Convert to feed iterator
            using var linqFeed = matches.ToFeedIterator();
            // Read the next page of results
            var results = new List<ImageTagDto>();
            while (linqFeed.HasMoreResults)
            {
                results.AddRange(await linqFeed.ReadNextAsync());
            }
            // Delete the item
            if (results.Count == 0)
            {
                return false;
            }
            await _cosmosContainer.DeleteItemAsync<ImageTagDto?>(results[0].id, new PartitionKey(results[0].id));

            return true;
        }
    }
}
