
using Microsoft.Azure.Cosmos;
using Ovotrack4.ImageTag.Service.Interfaces;
using Ovotrack4.ImageTag.Service.Services;


namespace Ovotrack4.ImageTag.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Get connection string, database name and container names from configuration
            // initialize cosmos database and containers if they don't exist


            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            var connectionString = configuration["CosmosDbConnectionString"];
            var databaseName = configuration["CosmosDbDatabaseName"];
            var imageTagsContainerName = configuration["CosmosDbImageTagsContainerName"];
                var imageMappingsContainerName = configuration["CosmosDbImageMappingsContainerName"];
                var partitionKeyPath = configuration["CosmosDbPartitionKeyPath"];
                var cosmosClient = new CosmosClient(connectionString);

           
            var database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName).Result;
            var imageTagsContainer = database.Database.CreateContainerIfNotExistsAsync(imageTagsContainerName, partitionKeyPath).Result;
            var imageMappingsContainer = database.Database.CreateContainerIfNotExistsAsync(imageMappingsContainerName, partitionKeyPath).Result;
            
            builder.Services.AddSingleton(cosmosClient);
            builder.Services.AddSingleton(database);
            builder.Services.AddSingleton(imageTagsContainer);
            builder.Services.AddSingleton(imageMappingsContainer);
            builder.Services.AddScoped<IImageVariableService, ImageVariableService>();
            builder.Services.AddScoped<IImageTagsService, ImageTagsService>();
            builder.Services.AddTransient<IImageVariableTagService, ImageVariableTagService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
           // if (app.Environment.IsDevelopment())
         //   {
                app.UseSwagger();
                app.UseSwaggerUI();
           // }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}