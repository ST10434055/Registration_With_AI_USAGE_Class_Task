using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Azure.Data.Tables;
using ProfileRegistratrionWithAiUsage.Models;
using System.Text.Json;

//fully utilised Claude Sonnet 4 (Anthropic) to generate this Azure Function
namespace ProfileRegistratrionWithAiUsage.Functions
{
    public class GetAllProfilesFunction
    {
        private readonly ILogger<GetAllProfilesFunction> _logger;
        private readonly TableServiceClient _tableServiceClient;

        public GetAllProfilesFunction(ILogger<GetAllProfilesFunction> logger, TableServiceClient tableServiceClient)
        {
            _logger = logger;
            _tableServiceClient = tableServiceClient;
        }

        [Function("GetAllProfiles")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "profile/all")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("GetAllProfiles Azure Function triggered");

                const string tableName = "Profiles";
                var tableClient = _tableServiceClient.GetTableClient(tableName);
                
                // Query all profiles (will return empty if table doesn't exist)
                var profiles = new List<ProfileTableEntity>();
                try
                {
                    await foreach (var profile in tableClient.QueryAsync<ProfileTableEntity>())
                    {
                        profiles.Add(profile);
                    }
                }
                catch (Azure.RequestFailedException ex) when (ex.Status == 404)
                {
                    _logger.LogWarning("Profiles table does not exist yet");
                    profiles = new List<ProfileTableEntity>();
                }

                _logger.LogInformation($"Retrieved {profiles.Count} profiles from table storage");

                // Return success response with profiles
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync(JsonSerializer.Serialize(new { 
                    success = true, 
                    message = "Profiles retrieved successfully",
                    count = profiles.Count,
                    profiles = profiles
                }));

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving profiles");
                
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync(JsonSerializer.Serialize(new { 
                    success = false, 
                    message = "An error occurred while retrieving profiles",
                    error = ex.Message
                }));

                return errorResponse;
            }
        }
    }
}
